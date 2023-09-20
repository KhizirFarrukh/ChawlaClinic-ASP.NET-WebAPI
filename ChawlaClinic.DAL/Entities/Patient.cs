using System.ComponentModel.DataAnnotations;

namespace ChawlaClinic.DAL.Entities
{
    public class Patient : Base
    {
        [Key]
        public new Guid Id { get; set; } = new Guid();
        public string Name { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public int Age { get; set; }
        public char Gender { get; set; } = char.MinValue;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CaseNo { get; set; } = string.Empty;
        public char Type { get; set; } = char.MinValue;
        public string Disease { get; set; } = string.Empty;
        public DateOnly FirstVisit { get; set; }
        public string DiscountMode { get; set; } = string.Empty;
    }
}
