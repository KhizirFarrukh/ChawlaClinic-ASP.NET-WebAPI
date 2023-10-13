using ChawlaClinic.BL.DTOs.Auth;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IAuthServiceRepo
    {
        (bool, int?) Login(LoginDTO dto);
        void setToken(int userId, string JwtToken);
        void clearToken(int userId);
    }
}
