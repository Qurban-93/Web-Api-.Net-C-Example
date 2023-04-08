namespace FirstAPI.Dtos.ProductDtos
{
    public class ProductReturnDto
    {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public double? SalePrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
