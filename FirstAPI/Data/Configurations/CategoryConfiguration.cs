using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstAPI.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p=>p.CategoryName).IsRequired(true).HasMaxLength(20);
            builder.Property(p => p.CategoryDescription).IsRequired(true);
        }
    }
}
