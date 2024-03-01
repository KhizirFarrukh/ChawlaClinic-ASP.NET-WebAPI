using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.BL.Services;

namespace ChawlaClinic.API
{
    public static class ServiceExtensions
    {
        public static void AddServiceScopes(IServiceCollection services)
        {
            services.AddScoped<IPatientServiceRepo, PatientServiceRepo>();
            services.AddScoped<IPaymentServiceRepo, PaymentServiceRepo>();
        }
    }
}
