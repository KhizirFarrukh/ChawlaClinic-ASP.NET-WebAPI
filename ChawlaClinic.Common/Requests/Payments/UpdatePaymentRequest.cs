namespace ChawlaClinic.Common.Requests.Payment
{
    public class UpdatePaymentRequest
    {
        public int PaymentId { get; set; }
        public int AmountPaid { get; set; }
        public int DiscountId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
