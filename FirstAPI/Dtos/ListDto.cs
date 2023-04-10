namespace FirstAPI.Dtos
{
    public class ListDto<T>
    {
        public List<T>? List { get; set; }
        public int TotalCount { get; set; }
    }
}
