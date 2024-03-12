using ChawlaClinic.Common.Responses.Discounts;

namespace ChawlaClinic.Common.Responses.Payments
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public string Code { get; set; } = null!;
        public int AmountPaid { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; } = null!;
        public DiscountResponse Discount { get; set; } = null!;
    }
}
