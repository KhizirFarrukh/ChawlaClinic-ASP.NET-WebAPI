using ChawlaClinic.API.Models;
using ChawlaClinic.BL.DTOs.User;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic.API.Controller
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserServiceRepo _userRepo;
        public UserController(IUserServiceRepo userRepo)
        {
            _userRepo = userRepo;
        }
        
        [HttpPost("AddUser")]
        public IActionResult AddUser(AddUserDTO dto)
        {
            try
            {
                (bool status, string message) = _userRepo.AddUser(dto);
                return Ok(new JSONResponse { Status = status, Message = message });
            }
            catch(Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
