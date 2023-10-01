namespace ChawlaClinic.BL.DTOs.Payment
{
    public class GetPaymentDTO
    {
        public string PaymentId { get; set; }
        public string PatientId { get; set; }
        public int AmountPaid { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}
