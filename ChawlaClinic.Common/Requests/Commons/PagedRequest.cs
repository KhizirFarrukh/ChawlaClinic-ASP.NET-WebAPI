namespace ChawlaClinic.Common.Requests.Commons
{
    public class PagedRequest
    {
        public int Size { get; set; }
        public int Page {  get; set; }
        public bool IsAscending { get; set; }
        public string SortColumn { get; set; }

        public PagedRequest(string sortColumn)
        {
            Size = 10;
            Page = 1;
            IsAscending = true;
            SortColumn = sortColumn;
        }

        public PagedRequest(int? size, int? page, bool? isAscending, string sortColumn)
        {
            Size = size ?? 10;
            Page = page ?? 1;
            IsAscending = isAscending ?? true;
            SortColumn = sortColumn;
        }

        public string GetSortingString() => IsAscending ? "ascending" : "descending";
    }
}
