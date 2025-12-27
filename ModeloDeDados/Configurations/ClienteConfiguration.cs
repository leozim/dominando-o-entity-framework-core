using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModeloDeDados.Domain;

namespace ModeloDeDados.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.OwnsOne(end => end.Endereco, e =>
            {
                // posso fazer o mesmo com os demais campos
                e.Property(p => p.Bairro).HasColumnName("Bairro");
                // e.ToTable("Endereco"); // table split
            });
    }
}