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
        // RelacionamentoUmParaUm();
        // RelacionamentoUmParaMuitos();
        // RelacionamentoMuitosParaMuitos();
        ExemploTPH(); // Table Per Hierarchy
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
    
    private static void RelacionamentoUmParaMuitos()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "São Paulo",
                Governador = new Governador {Nome = "Leonel Brizola", Partido = "PDT"}
            };

            db.Estados.Add(estado);

            db.SaveChanges();
        }

        using (var db = new ApplicationContext())
        {
            var estados = db.Estados.ToList();
            
            estados[0].Cidades.Add(new Cidade { Nome = "Sorocaba" });

            db.SaveChanges();

            foreach (var estado in db.Estados.Include(p => p.Cidades).AsNoTracking())
            {
                Console.WriteLine($"Estado: {estado.Nome}, Governador: {estado.Governador.Nome}");

                foreach (var estadoCidade in estado.Cidades)
                {
                    Console.WriteLine($"\t Cidade: {estadoCidade.Nome}");
                }
            }
        }
    }
    
    private static void RelacionamentoMuitosParaMuitos()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var ator1 = new Ator { Nome = "Rafael" };
            var ator2 = new Ator { Nome = "Pires" };
            var ator3 = new Ator { Nome = "Bruno" };

            var filme1 = new Filme { Descricao = "A volta dos que não foram" };
            var filme2 = new Filme { Descricao = "De volta para o futuro" };
            var filme3 = new Filme { Descricao = "Poeira em alto mar filme" };

            ator1.Filmes.Add(filme1);
            ator1.Filmes.Add(filme2);

            ator2.Filmes.Add(filme1);

            filme3.Atores.Add(ator1);
            filme3.Atores.Add(ator2);
            filme3.Atores.Add(ator3);

            db.AddRange(ator1, ator2, filme3);

            db.SaveChanges();

            foreach (var ator in db.Atores.Include(e => e.Filmes))
            {
                Console.WriteLine($"Ator: {ator.Nome}");

                foreach (var filme in ator.Filmes)
                {
                    Console.WriteLine($"\tFilme: {filme.Descricao}");
                }
            }
        }
    }
    
    private static void ExemploTPH()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var pessoa = new Pessoa { Nome = "Fulano de Tal" };

            var instrutor = new Instrutor { Nome = "Rafael Almeida", Tecnologia = ".NET", Desde = DateTime.Now };

            var aluno = new Aluno { Nome = "Maria Thysbe", Idade = 31, DataContrato = DateTime.Now.AddDays(-1) };

            db.AddRange(pessoa, instrutor, aluno);
            db.SaveChanges();

            var pessoas = db.Pessoas.AsNoTracking().ToArray();
            var instrutores = db.Instrutores.OfType<Instrutor>().AsNoTracking().ToArray();
            //var alunos = db.Alunos.AsNoTracking().ToArray();
            var alunos = db.Pessoas.OfType<Aluno>().AsNoTracking().ToArray();
            
            // o que eu queria fazer nao funcinou
            List<object> discriminators = new List<object>();
            foreach (var p in db.Pessoas.FromSqlRaw("SELECT Id, Nome, Desde, Tecnologia, Idade, TipoPessoa, DataContrato FROM Pessoas")) discriminators.Add(p);

            Console.WriteLine("Pessoas **************");
            foreach (var p in pessoas)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Nome}");
            }

            Console.WriteLine("Instrutores **************");
            foreach (var p in instrutores)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Tecnologia: {p.Tecnologia}, Desde: {p.Desde}");
            }

            Console.WriteLine("Alunos **************");
            foreach (var p in alunos)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Nome}, Idade: {p.Idade}, Data do Contrato: {p.DataContrato}");
            }
            
            Console.WriteLine("Discriminator *********************");
            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            discriminators.ForEach(d =>
            {
                var json = System.Text.Json.JsonSerializer.Serialize(d, options);
                Console.WriteLine(json);
            });
        }
    }
}
