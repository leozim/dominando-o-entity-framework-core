using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using ModeloDeDados.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModeloDeDados.Configurations;
using ModeloDeDados.Converters;

namespace ModeloDeDados.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Conversor> Conversores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    // public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Ator> Atores { get; set; }
    public DbSet<Filme> Filmes { get; set; }

    protected override  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string strConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ModeloDeDados;Integrated Security=True;pooling=True";

        optionsBuilder
            .UseSqlServer(
                strConnection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Departamento>().HasQueryFilter(dep => !dep.Excluido);
        // modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");
        // CI/CS significa Case Sensitive e Insensitive.
        // AI/AS para acentuações
        // modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CI_AI"); // usar apenas em uma propriedade.
        
        /*modelBuilder
            .HasSequence<int>("MinhaSequencia", "sequencias")
            .StartsAt(1)
            .IncrementsBy(2)
            .HasMin(1)
            .HasMax(10)
            .IsCyclic();
        modelBuilder.Entity<Departamento>().Property(p=>p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");
        */
        
        /*
        modelBuilder
            .Entity<Departamento>()
            .HasIndex(p=> new { p.Descricao, p.Ativo})
            .HasDatabaseName("idx_meu_indice_composto")
            .HasFilter("Descricao IS NOT NULL")
            .HasFillFactor(80)
            .IsUnique();
        */

        /*modelBuilder.Entity<Estado>().HasData(new[]
        {
            new Estado { Id = 1, Nome = "Ceará" },
            new Estado { Id = 2, Nome = "Rio Grande do Norte" }
        });*/
        
        /*
        modelBuilder.HasDefaultSchema("cadastros");
        modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");
        */
        
        /*
        var conversao = new ValueConverter<Versao, string>(p => p.ToString(), v => Enum.Parse<Versao>(v));
        
        // Vem do namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
        var conversao1 = new EnumToStringConverter<Versao>();
        
        modelBuilder.Entity<Conversor>()
            .Property(p => p.Versao)
            .HasConversion(conversao1);
            // .HasConversion(conversao);
            // .HasConversion(p => p.ToString(), v => Enum.Parse<Versao>(v));
            // .HasConversion(p => p.ToString(), v => (Versao)Enum.Parse(typeof(Versao), v)); // Código do instrutor
            // .HasConversion<string>();

            modelBuilder.Entity<Conversor>()
                .Property(p => p.Status)
                .HasConversion(new ConversorCustomizado());
                
        modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");
        */

        /*modelBuilder.Entity<Cliente>(p =>
        {
            p.OwnsOne(end => end.Endereco, e =>
            {
                // posso fazer o mesmo com os demais campos
                e.Property(p => p.Bairro).HasColumnName("Bairro");
                // e.ToTable("Endereco"); // table split
            });
        });*/

        // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}