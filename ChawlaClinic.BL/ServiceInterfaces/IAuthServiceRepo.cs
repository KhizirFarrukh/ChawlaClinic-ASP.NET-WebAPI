using ChawlaClinic.BL.DTOs.Auth;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IAuthServiceRepo
    {
        bool Login(LoginDTO dto);
    }
}
