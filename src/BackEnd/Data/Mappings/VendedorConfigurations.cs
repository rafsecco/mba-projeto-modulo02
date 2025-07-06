using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class VendedorConfigurations : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.HasKey(s => s.UserId);
            builder.Property(p => p.Ativo).HasDefaultValue(true);
        }
    }
}