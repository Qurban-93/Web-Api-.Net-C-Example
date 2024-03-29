﻿namespace FirstAPI.Models
{
    public class Category : BaseEntity
    {
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set;}
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<Product>? Products { get; set; }
        

    }
}
