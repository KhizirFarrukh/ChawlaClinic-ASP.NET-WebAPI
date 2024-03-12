using ChawlaClinic.Common.Requests.Commons;

namespace ChawlaClinic.Common.Requests.Payment
{
    public class GetPaymentsByPatientIdRequest : PagedRequest
    {
        public int PatientId { get; set; }

        public GetPaymentsByPatientIdRequest() : base("PaymentId") { }

        public GetPaymentsByPatientIdRequest(int patientId, int? size, int? page, bool? isAscending, string sortColumn) 
            : base(size, page, isAscending, sortColumn)
        {
            PatientId = patientId;
        }
    }
}
