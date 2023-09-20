using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.BL.Services;

namespace ChawlaClinic.API
{
    public static class ServiceExtensions
    {
        public static void AddServiceScopes(IServiceCollection services)
        {
            services.AddScoped<IUserServiceRepo, UserServiceRepo>();
            services.AddScoped<IAuthServiceRepo, AuthServiceRepo>();
            services.AddScoped<IPatientServiceRepo, PatientServiceRepo>();
        }
    }
}
