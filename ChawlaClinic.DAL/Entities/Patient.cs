using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChawlaClinic.DAL.Entities
{
    public class Patient : Base_ID_GUID
    {
        public string Name { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; } = char.MinValue;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CaseNo { get; set; } = string.Empty;
        public char Type { get; set; } = char.MinValue;
        public string Disease { get; set; } = string.Empty;
        public DateOnly FirstVisit { get; set; }

        public ICollection<Payment>? Payments { get; set; }

        [ForeignKey("PatientDiscount")]
        public int? DiscountId { get; set; }

        public PatientDiscount? PatientDiscount { get; set; }
    }
}
