using ChawlaClinic.Common.Requests.Payment;
using ChawlaClinic.Common.Responses.Payments;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPaymentServiceRepo
    {
        Task<bool> AddPayment(CreatePaymentRequest request);
        Task<bool> DeletePayment(int paymentId);
        Task<GetPaymentResponse?> GetPaymentByPaymentCode(string paymentCode);
        Task<GetPaymentResponse?> GetPaymentByPaymentId(int paymentId);
        Task<List<GetPaymentResponse>?> GetPaymentsByPatientId(GetPaymentsByPatientIdRequest request);
        Task<bool> UpdatePayment(UpdatePaymentRequest request);
    }
}
