namespace ChawlaClinic.BL.DTOs.Patient
{
    public class AddEmergencyBurnPatientDTO
    {
        public string Name { get; set; } = string.Empty;
        public string GuardianName { get; set; } = string.Empty;
        public int AgeYears { get; set; }
        public int AgeMonths { get; set; }
        public char Gender { get; set; } = char.MinValue;

    }
}
