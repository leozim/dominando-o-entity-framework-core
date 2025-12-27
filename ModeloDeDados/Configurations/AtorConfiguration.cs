using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModeloDeDados.Domain;

namespace ModeloDeDados.Configurations;

public class AtorConfiguration : IEntityTypeConfiguration<Ator>
{
    public void Configure(EntityTypeBuilder<Ator> builder)
    {
        /*builder
            .HasMany(a => a.Filmes)
            .WithMany(f => f.Atores);*/
        
        /*builder
            .HasMany(a => a.Filmes)
            .WithMany(f => f.Atores)
            .UsingEntity(p => p.ToTable("AtoresFilmes"));*/

        builder
            .HasMany(a => a.Filmes)
            .WithMany(f => f.Atores)
            .UsingEntity<Dictionary<string, object>>(
                "FilmesAtores",
                f => f.HasOne<Filme>().WithMany().HasForeignKey("FilmeId"),
                a => a.HasOne<Ator>().WithMany().HasForeignKey("AtorId"),
                p =>
                {
                    p.Property<DateTime>("CadastradoEm").HasDefaultValueSql("GETDATE()");
                }
            );
    }
}