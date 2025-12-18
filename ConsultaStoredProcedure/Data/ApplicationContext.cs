using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }

    protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ConsultaStoredProcedure;Integrated Security=True;pooling=True";

        optionsBuilder
            .UseSqlServer(strConnection/*, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery) */)
            .EnableSensitiveDataLogging()
            // .UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Departamento>().HasQueryFilter(dep => !dep.Excluido);
    }
}