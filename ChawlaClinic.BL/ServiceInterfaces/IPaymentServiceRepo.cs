using ChawlaClinic.BL.Requests.Payment;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPaymentServiceRepo
    {
        bool AddPayment(CreatePaymentRequest dto);
        List<GetPaymentByPaymentIdRequest>? GetPayments(string patientId);
        GetPaymentByPaymentIdRequest? GetPaymentById(string paymentId);
    }
}
