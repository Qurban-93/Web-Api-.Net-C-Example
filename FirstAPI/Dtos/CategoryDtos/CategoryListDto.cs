namespace FirstAPI.Dtos.CategoryDtos
{
    public class CategoryListDto
    {
        public List<CategoryReturnDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
