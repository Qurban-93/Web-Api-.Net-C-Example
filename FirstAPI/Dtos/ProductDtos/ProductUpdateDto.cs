namespace FirstAPI.Dtos.ProductDtos
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public IFormFile? Photo { get; set; }
        public double? SalePrice { get; set; }
        public bool isDelete { get; set; }
    }
}
