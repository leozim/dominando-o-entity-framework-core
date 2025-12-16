using System.Diagnostics;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

internal class Program
{
    private static void Main(string[] args)
    {
        // EnsureCreatedAndDelete();
        // GapOfEnsureCreated();
        // HealthCheckDataBase();
        
        // warmup
        // new ApplicationContext().Departamentos.AsNoTracking().Any();
        // _count = 0;
        // GerenciarEstadoDaConexao(false);
        // _count = 0;
        // GerenciarEstadoDaConexao(true);

        // MigracoesPendentes();

        // ScriptGeralDoBd();

        CarregamentoAdiantado();
    }
    
    static void CarregamentoExplicito()
        {
            using var db = new ApplicationContext();
            SetupTiposCarregamentos(db);

            var departamentos = db
                .Departamentos
                // ToList() carrega os dados na memória de forma antecipada fechando a conexão da consulta.
                // com isso "burlamos" a necessidade de ativar o Mars ao final da connection string com:
                // MultipleActiveResultSets=True
                // Permite varios lotes de consulta em uma úinica conexão
                .ToList();

            foreach (var departamento in departamentos)
            {
                /* Usei Guid ao inves de int */
                
                // if(departamento.Id == 2)
                // {
                //     //db.Entry(departamento).Collection(p=>p.Funcionarios).Load();
                //     db.Entry(departamento).Collection(p=>p.Funcionarios).Query().Where(p=>p.Id > 2).ToList();
                // }

                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
                }
            }
        }

        static void CarregamentoAdiantado()
        {
            using var db = new ApplicationContext();
            SetupTiposCarregamentos(db);

            var departamentos = db
                .Departamentos
                .Include(p => p.Funcionarios);

            foreach (var departamento in departamentos)
            {

                Console.WriteLine("---------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado!");
                }
            }
        }

        static void SetupTiposCarregamentos(ApplicationContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Rafael Almeida",
                                Cpf = "99999999911",
                            }
                        }
                    },
                    new Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                Cpf = "88888888811",
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                Cpf = "77777777711",
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

    private static void EnsureCreatedAndDelete()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
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

    private static void SqlInjection()
    {
        using var dbApp = new ApplicationContext();
        dbApp.Database.EnsureDeleted();
        dbApp.Database.EnsureCreated();

        dbApp.Departamentos.AddRange(
            new Departamento
            {
                Descricao = "Departamento 1",
            },
            new Departamento
            {
                Descricao = "Departamento 2",
            });
        dbApp.SaveChanges();
        
        //var descricao = "Teste ' or 1='1";
        //db.Database.ExecuteSqlRaw("update departamentos set descricao='AtaqueSqlInjection' where descricao={0}",descricao);
        //db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSqlInjection' where descricao='{descricao}'");

        foreach (var departamento in dbApp.Departamentos.AsNoTracking())
        {
            Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
        }
    }

    private static void ExecuteSQL()
    {
        using var dbApp = new ApplicationContext();
        
        // Primeira opção
        using (var cmd = dbApp.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteNonQuery();
        }
        
        // Segunda opção
        var descricao = "TESTE";
        dbApp.Database.ExecuteSqlRaw("UPDATE Departamenos SET descricao={0} WHERE id=1", descricao);
        
        // Terceira opção
        dbApp.Database.ExecuteSqlInterpolated($"UPDATE Departamenos SET descricao={descricao} WHERE id=1");
    }

    private static void AplicarMigracaoEmTempoDeExecucao()
    {
        using var dbApp = new ApplicationContext();
        
        dbApp.Database.Migrate();
    }

    private static void MigracoesPendentes()
    {
        using var dbApp = new ApplicationContext();
        using var dbAppCidade = new ApplicationContextCidade();
        ;
        
        var migracoesPendentes = dbApp.Database.GetPendingMigrations();
        var migracoesPendentesCidade = dbAppCidade.Database.GetPendingMigrations();

        Console.WriteLine($"Total migrações pendentes: {migracoesPendentes.Count()}");
        Console.WriteLine($"Total migracções pendentes Cidade Context: {migracoesPendentesCidade.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }

        foreach (var migracao in migracoesPendentesCidade)
        {
            Console.WriteLine($"Migração Cidade Context: {migracao}");
        }
    }
    
    static void AplicarMigracaoEmTempodeExecucao()
    {
        using var db = new ApplicationContext();

        db.Database.Migrate();
    }
    
    static void TodasMigracoes()
    {
        using var db = new ApplicationContext();

        var migracoes = db.Database.GetMigrations();

        Console.WriteLine($"Total: {migracoes.Count()}");

        foreach (var migracao in migracoes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }
    
    static void MigracoesJaAplicadas()
    {
        using var db = new ApplicationContext();

        var migracoes = db.Database.GetAppliedMigrations();

        Console.WriteLine($"Total: {migracoes.Count()}");

        foreach (var migracao in migracoes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }

    private static void ScriptGeralDoBd()
    {
        using var dbApp = new ApplicationContext();
        var script = dbApp.Database.GenerateCreateScript();

        Console.WriteLine(script);
    }
}