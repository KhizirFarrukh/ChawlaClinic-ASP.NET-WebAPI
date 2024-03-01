using ChawlaClinic.Common.Responses.Discounts;

namespace ChawlaClinic.Common.Responses.Payments
{
    public class GetPaymentResponse
    {
        public int PaymentId { get; set; }
        public int AmountPaid { get; set; }
        public DateTime DateTime { get; set; }
        public DiscountResponse Discount { get; set; } = null!;
    }
}
