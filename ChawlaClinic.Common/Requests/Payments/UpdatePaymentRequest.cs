namespace ChawlaClinic.Common.Requests.Payment
{
    public class UpdatePaymentRequest
    {
        public int PaymentId { get; set; }
        public int AmountPaid { get; set; }
        public DateOnly PaymentDate { get; set; }
    }
}
