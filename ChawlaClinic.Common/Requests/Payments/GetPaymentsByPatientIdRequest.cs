using ChawlaClinic.Common.Requests.Commons;

namespace ChawlaClinic.Common.Requests.Payment
{
    public class GetPaymentsByPatientIdRequest(int patientId, int? size, int? page, bool? isAscending, string sortColumn) 
        : PagedRequest(size, page, isAscending, sortColumn)
    {
        public int PatientId { get; set; } = patientId;
    }
}
