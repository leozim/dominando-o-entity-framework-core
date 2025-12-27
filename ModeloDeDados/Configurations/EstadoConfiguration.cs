using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModeloDeDados.Domain;

namespace ModeloDeDados.Configurations;

public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder
            .HasOne(e => e.Governador)
            .WithOne(g => g.Estado)
            .HasForeignKey<Governador>(g => g.EstadoId);
        
        // Com o AutoInclude() não é necessário usar o Include() através do dbContext para
        // o carregamento adiantado(eager).
        builder.Navigation(e => e.Governador).AutoInclude();

        builder
            .HasMany(e => e.Cidades)
            .WithOne(c => c.Estado)
            .IsRequired(); // permite inserir Cidade sem a existencia de um Estado caso passe 'false' como parametro
        // .OnDelete(DeleteBehavior.Restrict);
    }
}