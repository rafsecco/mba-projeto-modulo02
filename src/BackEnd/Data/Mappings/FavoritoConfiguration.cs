using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings;

public class FavoritoConfiguration : IEntityTypeConfiguration<Favorito>
{
    public void Configure(EntityTypeBuilder<Favorito> builder)
    {
        builder.HasKey(e => new { ClientId = e.ClienteId, e.ProdutoId });

        builder.HasOne(e => e.Cliente)
            .WithMany(e => e.Favoritos)
            .HasForeignKey(e => e.ClienteId);

        builder.HasOne(e => e.Produto)
            .WithMany(e => e.Favoritos)
            .HasForeignKey(e => e.ProdutoId);
    }
}