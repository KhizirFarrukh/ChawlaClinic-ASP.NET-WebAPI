namespace ChawlaClinic.Common.Responses.Patients
{
    public class PatientSearchResponse
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public string CaseNo { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime FirstVisit { get; set; }
    }
}
