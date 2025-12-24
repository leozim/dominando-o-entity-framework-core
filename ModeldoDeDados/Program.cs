// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using ModeldoDeDados.Data;
internal class Program
{
    public static void Main(string[] args)
    {
        // PropagarDados();
        ConversorDeValor();
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
}
