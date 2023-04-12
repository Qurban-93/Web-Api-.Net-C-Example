namespace FirstAPI.Dtos.ProductDtos
{
    public class ProductReturnDto
    {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string? ImageUrl { get; set; }
        public double Profit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public CategoryForProductReturnDto Category { get; set; }
    }

    public class CategoryForProductReturnDto
    {
        public int Id { get; set; }
        public string? CategoryName { get; set;}

    }
}
