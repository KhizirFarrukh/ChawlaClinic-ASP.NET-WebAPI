using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Responses.Discounts;

namespace ChawlaClinic.Common.Responses.Patients
{
    public class PatientResponse
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public string Gender { get; set; } = null!;
        public char Type { get; set; }
        public string? Disease { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string CaseNo { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime FirstVisit { get; set; }
        public DiscountResponse? Discount {  get; set; }
    }
}
