using ChawlaClinic.BL.DTOs.User;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.Common.Helpers;
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
        
        public GetUserDTO? GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.IsActive && !u.IsDeleted);
            
            if(user == null) { return null; }

            return new GetUserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                AddedOn = user.AddedOn,
                ModifiedOn = user.ModifiedOn
            };
        }
        public List<GetUserDTO> GetUsers()
        {
            var users = _context.Users.Where(u => u.IsActive && !u.IsDeleted).ToList();

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
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    IsDeleted = user.IsDeleted,
                    AddedOn = user.AddedOn,
                    ModifiedOn = user.ModifiedOn
                });
            }

            return usersDtos;
        }
        public (bool, string) AddUser(AddUserDTO dto)
        {
            if (dto.Password1 != dto.Password2)
            {
                return (false, CustomMessage.PASSWORD_NOT_MATCH);
            }
            var user = _context.Users.FirstOrDefault(u => u.UserName == dto.UserName || u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
            if (user != null)
            {
                if (user.UserName == dto.UserName)
                {
                    return (false, CustomMessage.USERNAME_ALREADY_EXIST);
                }
                else if (user.Email == dto.Email)
                {
                    return (false, CustomMessage.EMAIL_ALREADY_EXIST);
                }
                else if (user.PhoneNumber == dto.PhoneNumber)
                {
                    return (false, CustomMessage.PHONENUMBER_ALREADY_EXIST);
                }
            }

            (byte[] PasswordHash, byte[] PasswordSalt) = CommonHelper.PasswordHashSalt(dto.Password1);

            _context.Users.Add(new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                IsActive = true,
                IsDeleted = false,
                AddedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            });

            _context.SaveChanges();

            return (true, string.Format(CustomMessage.ADDED_SUCCESSFULLY, "User"));
        }
        public bool UpdateUser(UpdateUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.Id);
            
            if (user == null) { return false; }

            user.UserName = dto.UserName;
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.ModifiedOn = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
        public bool DeleteUser(int Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);

            if(user == null) { return false; }

            user.IsActive = false;
            user.IsDeleted = true;

            _context.SaveChanges();

            return true;
        }
    }
}
