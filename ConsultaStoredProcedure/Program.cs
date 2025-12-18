// See https://aka.ms/new-console-template for more information

using System.Data;
using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        // FiltroGlobal();
        // IgnoreFiltroGlobal();
        // ConsultaProjetada();
        // ConsultaParametrizada();
        // EntendendoConsulta1NN1();
        // DivisaoDeConsulta();
        // CriarStoredProcedure();
        // InserirDadosViaProcedure();
        // CriarStoredProcedureDeConsulta();
        ConsultaViaProcedure();
    }

    private static void ConsultaViaProcedure()
    {
        using var context = new ApplicationContext();

        var dep = new SqlParameter
        {
            ParameterName = "@Descricao",
            Value = "Departamento"
        };

        var departamentos = context.Departamentos
            .FromSqlRaw("EXECUTE GetDepartamentos @Descricao", dep)
            .ToList();
        
        departamentos.ForEach(d => Console.WriteLine($"Descrição: {d.Descricao}"));
    }

    private static void CriarStoredProcedureDeConsulta()
    {
        var getDepartamentos = @"
CREATE OR ALTER PROCEDURE GetDepartamentos
    @Descricao VARCHAR(50)
AS
BEGIN
    SELECT * 
    FROM 
        Departamentos
    WHERE
        Descricao LIKE @Descricao + '%'
END
";
        
        using var context = new ApplicationContext();
        context.Database.ExecuteSqlRaw(getDepartamentos);
    }

    private static void InserirDadosViaProcedure()
    {
        using var context = new ApplicationContext();

        context.Database
            .ExecuteSqlRaw("EXEC InserirDepartamento @p0, @p1", "INSERT VIA PROCEDURE", false);
    }

    private static void CriarStoredProcedure()
    {
        var inserirDepartamento = @"
CREATE OR ALTER PROCEDURE InserirDepartamento
    @Descricao VARCHAR(50),
    @Ativo BIT
AS
BEGIN
    INSERT INTO
        Departamentos(Descricao, Ativo, Excluido)
    VALUES
        (@Descricao, @Ativo, 0)
END
        ";

        using var context = new ApplicationContext();

        context.Database.ExecuteSqlRaw(inserirDepartamento);
    }
    
    private static void DivisaoDeConsulta()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var departamentos = context.Departamentos
            .Include(p => p.Funcionarios)
            .Where(p => p.Id < 3)
            // .AsSplitQuery()
            // Quando SplitQuery estiver como default configurado no ApplicationContext através do modelBuilder
            // O método de extensão abaixo força o uso de query única com AsSingleQuery
            .AsSingleQuery()
            .ToList();
        
        departamentos.ForEach(dep =>
        {
            Console.WriteLine($"Descrição: {dep.Descricao}");
            dep.Funcionarios.ForEach(func => Console.WriteLine($"\t Nome: {func.Nome}"));
        });
    }

    private static void EntendendoConsulta1NN1()
    {
        using var context = new ApplicationContext();
        Setup(context);
        
        // 1:N
        var departamentos = context.Departamentos
            .Include(p => p.Funcionarios)
            .ToList();
        
        departamentos.ForEach(dep =>
        {
            Console.WriteLine($"Descrição: {dep.Descricao}");
            dep.Funcionarios.ForEach(func => Console.WriteLine($"\t Nome: {func.Nome}"));
        });
        
        // N:1
        var funcionarios = context.Funcionarios
            .Include(p => p.Departamento)
            .ToList();
        
        funcionarios.ForEach(func => 
            Console.WriteLine($"Nome: {func.Nome} \t Descrição Dep: {func.Departamento.Descricao}")
            );
    }
    
    private static void ConsultaComTAG()
    {
        using var db = new ApplicationContext();
        Setup(db);

        var departamentos = db.Departamentos
            .TagWith(@"Estou enviando um comentario para o servidor
                
                Segundo comentario
                Terceiro comentario")
            .ToList();

        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Descrição: {departamento.Descricao}");
        }
    }

    private static void ConsultaInterpolada()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var id = 1;
        var departamentos = context.Departamentos
            .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id>{id}")
            .ToList();

        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Descrição: {departamento.Descricao}");
        }
    }

    private static void ConsultaParametrizada()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var parameters = new object[]
        {
            new SqlParameter
            {
                Value = 1,
                SqlDbType = SqlDbType.Int
            },
            new SqlParameter
            {
                Value = true,
                SqlDbType = SqlDbType.Bit
            }
        };
        
        // Quando usa-se um array de object passado como 'parameters' o EFCore produz uma subquery dentro da query.
        var departamentos = context.Departamentos
            .FromSqlRaw("SELECT * FROM Departamentos WHERE Id > {0} AND Excluido <> {1}", parameters)
            .Where(p => !p.Excluido)
            .ToList();
        
        departamentos.ForEach(dep => Console.WriteLine($"Descrição: {dep.Descricao} \t Excluido: {dep.Excluido}"));
    }

    private static void ConsultaProjetada()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var departamentos = context.Departamentos
            .Where(dep => dep.Id > 0)
            .Select(dep => new
            {
                dep.Descricao, 
                Funcionarios = dep.Funcionarios.Select(func => func.Nome),
            })
            .ToList();
        
        departamentos.ForEach(dep =>
        {
            Console.WriteLine($"Descrição: {dep.Descricao}");
            dep.Funcionarios.ToList().ForEach(funcionario => Console.WriteLine($"\t Nome: {funcionario}"));
        });
    }
    
    private static void IgnoreFiltroGlobal()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var departamentos = context.Departamentos.IgnoreQueryFilters().Where(p => p.Id > 0).ToList();
        
        departamentos.ForEach(dep => Console.WriteLine($"{dep.Descricao} \t {dep.Excluido}"));
    }

    private static void FiltroGlobal()
    {
        using var context = new ApplicationContext();
        Setup(context);

        var departamentos = context.Departamentos.Where(p => p.Id > 0).ToList();
        
        departamentos.ForEach(dep => Console.WriteLine($"{dep.Descricao} \t {dep.Excluido}"));
    }

    private static void Setup(ApplicationContext context)
    {
        if (context.Database.EnsureCreated())
        {
            context.Departamentos.AddRange(
                new Departamento
                {
                    Ativo = true,
                    Descricao = "Departamento 01",
                    Funcionarios = new List<Funcionario>
                    {
                        new Funcionario
                        {
                            Nome = "Leonardo Mariz",
                            Cpf = "11133399966",
                            Rg = "22235417119"
                        }
                    },
                    Excluido = true
                },
                new Departamento
                {
                    Ativo = true,
                    Descricao = "Departamento 02",
                    Funcionarios = new List<Funcionario>
                    {
                        new Funcionario
                        {
                            Nome = "Novo Funcionario DEP 02",
                            Cpf = "55533399977",
                            Rg = "11135417888"
                        },
                        new Funcionario
                        {
                            Nome = "Outro Funcionario DEP 02",
                            Cpf = "44422299900",
                            Rg = "44435417777"
                        }
                    },
                    Excluido = false
                });

            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
