namespace ChawlaClinic.Common.Requests.Patient
{
    public class CreatePatientRequest
    {
        public string Name { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; }
        public char Type { get; set; }
        public string Disease { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? CaseNo { get; set; }
        public DateTime? FirstVisit { get; set; }
    }
}
