using System.ComponentModel.DataAnnotations.Schema;

namespace ChawlaClinic.BL.Requests.Patient
{
    public class GetPatientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; } = char.MinValue;
        public char Type { get; set; } = char.MinValue;
        public string Disease { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CaseNo { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateOnly FirstVisit { get; set; }
        public int? DiscountId { get; set; }
        public string Discount { get; set; } = string.Empty;
    }
}
