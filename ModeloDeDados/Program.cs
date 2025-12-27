// See https://aka.ms/new-console-template for more information

using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using ModeloDeDados.Data;
using ModeloDeDados.Domain;

internal class Program
{
    public static void Main(string[] args)
    {
        // PropagarDados();
        // ConversorDeValor();
        // ConversorCustomizado();
        // TrabalhandoComPropriedadesDeSombra();
        // TiposDePropriedades();
        RelacionamentoUmParaUm();
    }
    
    private static void Collation()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    private static void PropagarDados()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var script = db.Database.GenerateCreateScript();
        Console.WriteLine(script);
    }

    private static void Esquema()
    {
        using var db = new ApplicationContext();

        var script = db.Database.GenerateCreateScript();

        Console.WriteLine(script);
    }

    private static void ConversorDeValor() => Esquema();

    private static void ConversorCustomizado()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Conversores.Add(
            new Conversor
            {
                Status = Status.Devolvido,
            });
        db.SaveChanges();

        var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);
        
        var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
        
        Console.WriteLine($"{conversorDevolvido?.ToString()}");
    }

    private static void TrabalhandoComPropriedadesDeSombra()
    {
        using var db = new ApplicationContext();
        /*db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var departamento = new Departamento
        {
            Descricao = "Departamento Shadow Property"
        };

        db.Departamentos.Add(departamento);
        db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;
        db.SaveChanges();*/

        var departamentos = db.Departamentos
            .AsNoTracking()
            .Where(p => 
                EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now)
            .ToList();
    }

    private static void TiposDePropriedades()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var cliente = new Cliente
        {
            Nome = "Fulano de Tal",
            Telefone = "(79) 98888-9999",
            Endereco = new Endereco { Bairro = "Centro", Cidade = "Fortaleza" }
        };

        db.Clientes.Add(cliente);

        db.SaveChanges();
        
        var clientes = db.Clientes.AsNoTracking().ToList();

        var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        
        clientes.ForEach(cliente =>
        {
            var json = System.Text.Json.JsonSerializer.Serialize(cliente, options);
            Console.WriteLine(json);
        });
    }

    private static void RelacionamentoUmParaUm()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var estado = new Estado
        {
            Nome = "Rio de Janeiro",
            Governador = new Governador { Nome = "Leonel Brizola", Partido = "PDT"}
        };

        db.Estados.Add(estado);

        db.SaveChanges();

        var estados = db.Estados.AsNoTracking().ToList();
        
        estados.ForEach(est =>
        {
            Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
        });
    }
}
