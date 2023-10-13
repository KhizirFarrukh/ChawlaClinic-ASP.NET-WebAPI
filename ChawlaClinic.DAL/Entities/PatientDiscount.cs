namespace ChawlaClinic.DAL.Entities
{
    public class PatientDiscount : Base
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Patient>? Patients { get; set; }
    }
}
