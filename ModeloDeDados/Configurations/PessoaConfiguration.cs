using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModeloDeDados.Domain;

namespace ModeloDeDados.Configurations;

public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder
            .ToTable("Pessoas")
            .HasDiscriminator<int>("TipoPessoa")
            .HasValue<Pessoa>((int)TipoPessoa.Pessoa)
            .HasValue<Instrutor>((int)TipoPessoa.Instrutor)
            .HasValue<Aluno>((int)TipoPessoa.Aluno);
    }
}