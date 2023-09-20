namespace ChawlaClinic.BL.DTOs.Patient
{
    public class GetPatientForSearchDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CaseNo { get; set; }
        public string MobileNumber { get; set; }
        public DateOnly FirstVisit { get; set; }
        public bool Status { get; set; }
    }
}
