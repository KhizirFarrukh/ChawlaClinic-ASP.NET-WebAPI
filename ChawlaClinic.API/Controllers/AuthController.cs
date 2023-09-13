using Microsoft.AspNetCore.Mvc;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.BL.DTOs.Auth;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.API.Models;

namespace ChawlaClinic.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthServiceRepo _authRepo;
        public AuthController(IAuthServiceRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                bool status = _authRepo.Login(dto);
                string message = CustomMessage.LOGIN_SUCCESSFUL;

                if (!status)
                {
                    message = CustomMessage.INCORRECT_CREDENTIALS;
                }

                // Session maintaining code here

                return Ok(new JSONResponse { Status = status, Message = message });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
