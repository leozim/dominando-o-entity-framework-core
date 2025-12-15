using System.Diagnostics;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

internal class Program
{
    private static void Main(string[] args)
    {
        // EnsureCreatedAndDelete();
        // GapOfEnsureCreated();
        HealthCheckDataBase();

    }

    private static void EnsureCreatedAndDelete()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        // db.Database.EnsureCreated();
    }

    private static void GapOfEnsureCreated()
    {
        using var dbApp = new ApplicationContext();
        using var dbAppCidade = new ApplicationContextCidade();

        dbApp.Database.EnsureCreated();
        dbAppCidade.Database.EnsureCreated();

        var databaseCreator = dbAppCidade.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

    private static void HealthCheckDataBase()
    {
        using var dbApp = new ApplicationContext();
        var canConnect = dbApp.Database.CanConnect();

        if (canConnect)
        {
            Console.WriteLine("Posso me conectar");
        }
        else
        {
            Console.WriteLine("Não posso me conectar");
        }
    }

    private static int _count;

    private static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
    {
        using var dbApp = new ApplicationContext();
        var time = Stopwatch.StartNew();

        var connection = dbApp.Database.GetDbConnection();

        connection.StateChange += (_, __) => ++_count;

        if (gerenciarEstadoConexao)
        {
            connection.Open();
        }

        for (var i = 0; i < 200; i++)
        {
            dbApp.Departamentos.AsNoTracking().Any();
        }
        
        time.Stop();
        var message = $"Time: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Counter: {_count}";

        Console.WriteLine(message);
    }
}