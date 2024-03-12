namespace ChawlaClinic.Common.Requests.Payment
{
    public class CreatePaymentRequest
    {
        public int PatientId { get; set; }
        public int AmountPaid { get; set; }
        public DateTime? DateTime { get; set; }
        public bool PrintReceipt { get; set; }
    }
}
