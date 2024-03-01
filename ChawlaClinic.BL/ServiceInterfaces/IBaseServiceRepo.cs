using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Patients;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IBaseServiceRepo<T>
    {
        Task<int> GetSequence();
    }
}
