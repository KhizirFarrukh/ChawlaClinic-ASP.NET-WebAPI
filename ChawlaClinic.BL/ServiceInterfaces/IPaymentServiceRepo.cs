using ChawlaClinic.BL.DTOs.Payment;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPaymentServiceRepo
    {
        bool AddPayment(AddPaymentDTO dto);
    }
}
