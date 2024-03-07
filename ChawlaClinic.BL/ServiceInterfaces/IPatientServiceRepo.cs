using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Commons;
using ChawlaClinic.Common.Responses.Patients;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        Task<bool> AddPatient(CreatePatientRequest request);
        Task<bool> AddPatient(CreateEmergencyBurnPatientRequest request);
        Task<bool> DeletePatient(int patientId);
        Task<PatientResponse?> GetPatientById(int PatientId);
        Task<PaginatedList<PatientResponse>> GetPatients(PagedRequest request);
        Task<PaginatedList<PatientSearchResponse>> SearchPatient(SearchPatientRequest filters);
        Task<bool> UpdatePatient(UpdatePatientRequest request);
    }
}
