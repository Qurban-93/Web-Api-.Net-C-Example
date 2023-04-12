namespace FirstAPI.Dtos.ProductDtos
{
    public class ProductListItemDto
    {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public CategoryInProductListItemDto Category { get; set; }
    }

    public class CategoryInProductListItemDto
    {
        public int Id { get; set; }
        public string? CategoryName { get; set;}
        public int ProductsCount { get; set; }
    }
}
