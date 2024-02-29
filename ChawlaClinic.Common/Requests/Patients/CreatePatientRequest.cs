using ChawlaClinic.Common.Enums;

namespace ChawlaClinic.Common.Requests.Patient
{
    public class CreatePatientRequest
    {
        public string Name { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public Gender Gender { get; set; }
        public PatientType Type { get; set; }
        public string Disease { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CaseNo { get; set; } = null!;
        public DateOnly? FirstVisit { get; set; }
    }
}
