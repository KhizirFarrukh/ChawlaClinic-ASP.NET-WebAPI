using ChawlaClinic.BL.DTOs.User;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IUserServiceRepo
    {
        GetUserDTO? GetUserById(int id);
        List<GetUserDTO> GetUsers();
        (bool, string) AddUser(AddUserDTO dto);
        bool UpdateUser(UpdateUserDTO dto);
        bool DeleteUser(int id);
    }
}
