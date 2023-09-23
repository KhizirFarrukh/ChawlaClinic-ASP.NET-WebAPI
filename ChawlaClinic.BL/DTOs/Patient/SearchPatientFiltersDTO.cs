using ChawlaClinic.Common.Commons;

namespace ChawlaClinic.BL.DTOs.Patient
{
    public class SearchPatientFiltersDTO
    {
        public string SearchParam { get; set; } = string.Empty;
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public char Type { get; set; }
        public DateOnly? FirstVisitStart { get; set; }
        public DateOnly? FirstVisitEnd { get; set; }
        public FilterActiveStatus ActiveStatus { get; set; }
        public FilterDeleteStatus DeleteStatus { get; set; }
    }

    
}
