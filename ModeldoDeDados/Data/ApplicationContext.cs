using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModeloDeDados.Domain;

namespace ModeldoDeDados.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Infrastructure;Integrated Security=True;pooling=True";

        optionsBuilder
            .UseSqlServer(
                strConnection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Departamento>().HasQueryFilter(dep => !dep.Excluido);
    }
}