using ChawlaClinic.Common.Enums;

namespace ChawlaClinic.Common.Requests.Patient
{
    public class PatientSearchResponse
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string CaseNo { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public PatientStatus Status { get; set; }
        public DateOnly FirstVisit { get; set; }
    }
}
