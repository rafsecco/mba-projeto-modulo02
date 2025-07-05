using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class CategoriaConfigurations : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id).HasColumnType("integer").ValueGeneratedOnAdd();
            builder.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Descricao).IsRequired().HasMaxLength(300);
        }
    }
}