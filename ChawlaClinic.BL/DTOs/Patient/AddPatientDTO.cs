namespace ChawlaClinic.BL.DTOs.Patient
{
    public class AddPatientDTO
    {
        public string Name { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; } = char.MinValue;
        public char Type { get; set; } = char.MinValue;
        public string Disease { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CaseNo { get; set; } = string.Empty;
        public DateOnly FirstVisit { get; set; }
    }
}
