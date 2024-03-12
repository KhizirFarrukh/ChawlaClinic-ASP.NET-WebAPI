using ChawlaClinic.Common.Requests.Payment;
using ChawlaClinic.Common.Responses.Commons;
using ChawlaClinic.Common.Responses.Payments;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPaymentServiceRepo
    {
        Task<bool> AddPayment(CreatePaymentRequest request);
        Task<bool> DeletePayment(int paymentId);
        Task<PaymentResponse?> GetPaymentByPaymentCode(string paymentCode);
        Task<PaymentResponse?> GetPaymentByPaymentId(int paymentId);
        Task<PaginatedList<PaymentResponse>> GetPaymentsByPatientId(GetPaymentsByPatientIdRequest request);
        Task<bool> UpdatePayment(UpdatePaymentRequest request);
    }
}
