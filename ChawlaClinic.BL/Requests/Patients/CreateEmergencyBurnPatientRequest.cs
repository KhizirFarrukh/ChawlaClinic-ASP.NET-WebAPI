using ChawlaClinic.Common.Enums;

namespace ChawlaClinic.BL.Requests.Patient
{
    public class CreateEmergencyBurnPatientRequest
    {
        public string Name { get; set; } = null!;
        public string GuardianName { get; set; } = null!;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public Gender Gender { get; set; }

    }
}
