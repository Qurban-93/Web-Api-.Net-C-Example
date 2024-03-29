﻿using FirstAPI.Data.Configurations;
using FirstAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Data.DAL
{
    public class WebAppDbContext : IdentityDbContext<AppUser>
    { 
        public WebAppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
