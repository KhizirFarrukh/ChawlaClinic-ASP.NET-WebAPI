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

            _context.SaveChanges();
        }
        
        public GetUserDTO? GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            
            if(user == null) { return null; }

            return new GetUserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public List<GetUserDTO> GetUsers()
        {
            var users = _context.Users.ToList();

            var usersDtos = new List<GetUserDTO>();

            if (users == null) { return usersDtos; }

            foreach(var user in users)
            {
                usersDtos.Add(new GetUserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }

            return usersDtos;
        }

        private (byte[], byte[]) PasswordHashing(string password)
        {
            return (null, null);
        }
    }
}
