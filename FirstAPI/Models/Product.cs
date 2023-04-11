namespace FirstAPI.Models
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }       
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; } 
    }
}
