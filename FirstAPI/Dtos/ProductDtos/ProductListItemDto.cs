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
    }
}
