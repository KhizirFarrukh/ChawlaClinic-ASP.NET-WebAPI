using ChawlaClinic.BL.DTOs.User;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IUserServiceRepo
    {
        void AddUser(AddUserDTO dto);
        GetUserDTO? GetUserById(int id);
        List<GetUserDTO> GetUsers();
    }
}
