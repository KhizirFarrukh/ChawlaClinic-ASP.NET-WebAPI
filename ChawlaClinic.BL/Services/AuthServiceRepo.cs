using ChawlaClinic.BL.DTOs.Auth;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Helpers;
using ChawlaClinic.DAL;

namespace ChawlaClinic.BL.Services
{
    public class AuthServiceRepo : IAuthServiceRepo
    {
        ApplicationDbContext _context;
        public AuthServiceRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == dto.UserIdentifier || u.Email == dto.UserIdentifier || u.PhoneNumber == dto.UserIdentifier);

            if (user == null) { return false; }

            return CommonHelper.ValidatePassword(dto.Password, user.PasswordHash, user.PasswordSalt);
        }

    }
}
