namespace FirstAPI.Dtos.CategoryDtos
{
    public class CategoryReturnDto
    {
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductsCount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }


}
