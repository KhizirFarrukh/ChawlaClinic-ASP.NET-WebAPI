using System.ComponentModel.DataAnnotations.Schema;

namespace ChawlaClinic.DAL.Entities
{
    public class Payment : Base_ID_GUID
    {
        public int AmountPaid { get; set; }
        public DateOnly PaymentDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenID { get; set; }
        public string SecureToken { get; set; } = string.Empty;

        [ForeignKey("Patient")]
        public Guid PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}
