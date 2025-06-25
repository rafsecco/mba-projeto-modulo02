using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configurations
{
    public class ProdutoConfigurations : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);
            //builder.Property(c => c.Id).HasColumnType("integer").ValueGeneratedOnAdd();
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Descricao).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Ativo).HasDefaultValue(true);

            builder.HasOne(p => p.Vendedor)
                .WithMany(s => s.Produtos)
                .HasForeignKey(p => p.VendedorId);

            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}