using ChawlaClinic.Common.Commons;

namespace ChawlaClinic.BL.Requests.Patient
{
    public class SearchPatientFiltersDTO
    {
        public string SearchParam { get; set; } = null!;
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public char Type { get; set; }
        public DateOnly? FirstVisitStart { get; set; }
        public DateOnly? FirstVisitEnd { get; set; }
    }

    
}
