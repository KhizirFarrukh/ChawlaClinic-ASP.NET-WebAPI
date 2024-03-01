namespace ChawlaClinic.Common.Requests.Payment
{
    public class CreatePaymentRequest
    {
        public int PatientId { get; set; }
        public int AmountPaid { get; set; }
        public int DiscountId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
