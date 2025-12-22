using Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationContext : DbContext
{
    private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true);
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Infrastructure;Integrated Security=True;pooling=True";

        optionsBuilder
            .UseSqlServer(
                strConnection,
                o=> o
                    .MaxBatchSize(100)
                    .CommandTimeout(5)
                    .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null))
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Departamento>().HasQueryFilter(dep => !dep.Excluido);
    }

    public override void Dispose()
    {
        base.Dispose();
        _writer.Dispose();
    }
}