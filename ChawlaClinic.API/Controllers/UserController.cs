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

        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userRepo.GetUserById(id);
                return Ok(new JSONResponse { Status = true, Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var user = _userRepo.GetUsers();
                return Ok(new JSONResponse { Status = true, Data = user });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
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

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(UpdateUserDTO dto)
        {
            try
            {
                bool status = _userRepo.UpdateUser(dto);
                string message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "User");

                if (!status)
                {
                    message = string.Format(CustomMessage.NOT_FOUND, "User");
                }

                return Ok(new JSONResponse { Status = status, Message = message });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int Id)
        {
            try
            {
                bool status = _userRepo.DeleteUser(Id);
                string message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "User");

                if (!status)
                {
                    message = string.Format(CustomMessage.NOT_FOUND, "User");
                }

                return Ok(new JSONResponse { Status = status, Message = message });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
