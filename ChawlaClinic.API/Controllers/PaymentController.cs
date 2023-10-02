using ChawlaClinic.API.Models;
using ChawlaClinic.BL.DTOs.Payment;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic.API.Controllers
{
    public class PaymentController : ControllerBase
    {
        private IPaymentServiceRepo _paymentRepo;
        public PaymentController(IPaymentServiceRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpPost("AddPayment")]
        public IActionResult AddPayment(AddPaymentDTO dto)
        {
            try
            {
                bool result = _paymentRepo.AddPayment(dto);

                string msg = string.Format(CustomMessage.NOT_FOUND, "Patient");

                if(result)
                {
                    msg = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Payment");
                }

                return Ok(new JSONResponse { Status = result, Message = msg });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
