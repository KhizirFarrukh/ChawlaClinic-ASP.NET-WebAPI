using ChawlaClinic.Common.Enums;

namespace ChawlaClinic.Common.Requests.Patient
{
    public class UpdatePatientRequest
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public Gender Gender { get; set; }
        public PatientType Type { get; set; }
        public string Disease { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public PatientStatus Status { get; set; }
        public DateTime FirstVisit { get; set; }
        public int DiscountId { get; set; }
    }
}
