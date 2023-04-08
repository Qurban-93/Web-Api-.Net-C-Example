using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstAPI.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50).IsRequired(true);
            builder.Property(p=>p.SalePrice).IsRequired(true);
            builder.Property(p => p.EditDate).HasDefaultValue("0001 ,01 ,01");
        }
    }
}
