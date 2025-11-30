namespace EVCS.BlazorWebApp.TriNM.Models
{
    public class PaginationResult<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public T Items { get; set; }
    }
}
