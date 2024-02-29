using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Requests.Commons;

namespace ChawlaClinic.Common.Requests.Patient
{
    public class SearchPatientRequest : PagedRequest
    {
        public string SearchParam { get; set; } = null!;
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public PatientType? Type { get; set; }
        public DateOnly? FirstVisitStart { get; set; }
        public DateOnly? FirstVisitEnd { get; set; }
    }
}
