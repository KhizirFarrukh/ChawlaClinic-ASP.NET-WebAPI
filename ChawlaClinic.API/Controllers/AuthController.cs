using Microsoft.AspNetCore.Mvc;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.BL.DTOs.Auth;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ChawlaClinic.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IAuthServiceRepo _authRepo;
        private IUserServiceRepo _userRepo;

        public AuthController(IAuthServiceRepo authRepo, IUserServiceRepo userRepo, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _userRepo = userRepo;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                (bool status, int? userId) = _authRepo.Login(dto);
                string message;
                string JwtToken = "";

                if (status && userId != null)
                {
                    var user = _userRepo.GetUserById(userId ?? -1);

                    if(user == null)
                    {
                        message = message = string.Format(CustomMessage.NOT_FOUND, "User");
                    }
                    else
                    {
                        message = CustomMessage.LOGIN_SUCCESSFUL;

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                        };

                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:TokenExpirationTimeHours"] ?? "1")),
                            signingCredentials: credentials
                        );

                        JwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                        _authRepo.setToken(user.Id, JwtToken);
                    }
                }
                else
                {
                    message = CustomMessage.INCORRECT_CREDENTIALS;
                }

                return Ok(new JSONResponse { Status = status, Message = message, Data = JwtToken });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "-1");
                _authRepo.clearToken(userId);

                return Ok(new JSONResponse { Status = true, Message = CustomMessage.LOGOUT_SUCCESSFUL });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
