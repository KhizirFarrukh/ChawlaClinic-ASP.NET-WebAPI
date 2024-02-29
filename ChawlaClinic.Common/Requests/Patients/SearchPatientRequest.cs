using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Requests.Commons;

namespace ChawlaClinic.Common.Requests.Patient
{
    public class SearchPatientRequest : PagedRequest
    {
        public string SearchParam { get; set; } = null!;
        public PatientType? Type { get; set; }
        public PatientStatus? Status { get; set; }
        public DateOnly? FirstVisitStart { get; set; }
        public DateOnly? FirstVisitEnd { get; set; }

        public SearchPatientRequest(int? size, int? page, bool? isAscending, string? sortColumn) 
            : base(size, page, isAscending, sortColumn ?? "CaseNo")
        { }
    }
}
