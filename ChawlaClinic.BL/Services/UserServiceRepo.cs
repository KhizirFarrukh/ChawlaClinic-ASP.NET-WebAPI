using ChawlaClinic.BL.DTOs.User;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChawlaClinic.BL.Services
{
    public class UserServiceRepo : IUserServiceRepo
    {
        ApplicationDbContext _context;
        public UserServiceRepo(ApplicationDbContext context) 
        {
            _context = context;
        }
        public (bool, string) AddUser(AddUserDTO dto)
        {
            if(dto.Password1 != dto.Password2)
            {
                return (false, CustomMessage.PASSWORD_NOT_MATCH);
            }
            var user = _context.Users.FirstOrDefault(u => u.UserName == dto.UserName || u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
            if (user != null) 
            {
                if(user.UserName == dto.UserName)
                {
                    return (false, CustomMessage.USERNAME_ALREADY_EXIST);
                }
                else if(user.Email == dto.Email)
                {
                    return (false, CustomMessage.EMAIL_ALREADY_EXIST);
                }
                else if (user.PhoneNumber == dto.PhoneNumber)
                {
                    return (false, CustomMessage.PHONENUMBER_ALREADY_EXIST);
                }
            }

            byte[] PasswordHash;
            byte[] PasswordSalt;

            (PasswordHash, PasswordSalt) = PasswordHashing(dto.Password1);

            _context.Users.Add(new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            });
        }
        
        public GetUserDTO? GetUserById(int id)
        {

        }

        public List<GetUserDTO> GetUsers()
        {

        }

        private (byte[], byte[]) PasswordHashing(string password)
        {
            return (null, null);
        }
    }
}
