using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedido.Domain.Custumer.Entities;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.OwnsOne(c => c.Nome, nome =>
        {
            nome.Property(n => n.Value)
                .HasColumnName("Nome")
                .IsRequired()
                .HasMaxLength(150);
        });
    }
}
