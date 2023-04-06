using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Data.DAL
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }
    }
}
