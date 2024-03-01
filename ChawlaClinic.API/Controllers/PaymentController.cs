using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Requests.Payment;
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

        [HttpGet("GetPayments")]
        public async Task<IActionResult> GetPayments(GetPaymentsByPatientIdRequest request)
        {
            try
            {
                return Ok(await _paymentRepo.GetPaymentsByPatientId(request));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("GetPaymentByPaymentId")]
        public async Task<IActionResult> GetPaymentByPaymentId(int paymentId)
        {
            try
            {
                return Ok(await _paymentRepo.GetPaymentByPaymentId(paymentId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                bool result = await _paymentRepo.AddPayment(request);

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
