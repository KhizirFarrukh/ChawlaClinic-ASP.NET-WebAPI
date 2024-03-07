namespace ChawlaClinic.Common.Responses.Commons
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items {  get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PaginatedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;

            TotalPages = (int)Math.Ceiling((double)totalCount/ pageSize);

            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
        }
    }
}
