using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data;

public class ApplicationContextCidade : DbContext
{
    public DbSet<Cidade> Cidades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DominandoEFCore;Integrated Security=True;";
            
        optionsBuilder
            .UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}