using ChawlaClinic.BL.DTOs.User;
using ChawlaClinic.Common.Commons;
using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic.API.Controller
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("AddUser")]
        public IActionResult AddUser(AddUserDTO dto)
        {
            return Ok();
        }
    }
}
