namespace ChawlaClinic.BL.Responses.Patients
{
    public class GetPatientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; } = char.MinValue;
        public char Type { get; set; } = char.MinValue;
        public string Disease { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CaseNo { get; set; } = null!;
        public bool Status { get; set; }
        public DateOnly FirstVisit { get; set; }
        public int? DiscountId { get; set; }
        public string Discount { get; set; } = null!;
    }
}
