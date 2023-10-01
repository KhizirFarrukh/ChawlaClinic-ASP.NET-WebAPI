namespace ChawlaClinic.BL.DTOs.Payment
{
    public class UpdatePaymentDTO
    {
        public string PatientId { get; set; }
        public int AmountPaid { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}
