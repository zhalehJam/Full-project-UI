namespace Framework.Pagination
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 1;

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int ItemsStart => (CurrentPage * PageSize) - PageSize + 1;

        public int ItemsEnd()
        {
            var x = CurrentPage * PageSize;
            if (x > TotalCount) x = TotalCount;
            return x;
        }
    }
}