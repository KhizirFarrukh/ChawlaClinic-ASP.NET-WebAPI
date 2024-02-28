namespace ChawlaClinic.Common.Requests.Commons
{
    public class PagedRequest
    {
        public int? Size { get; set; }
        public int? Page {  get; set; }
        public bool? IsAscending { get; set; }
        public string? SortColumn { get; set; }
    }
}
