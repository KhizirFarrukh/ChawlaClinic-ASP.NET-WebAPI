using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Patients;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        Task<bool> AddPatient(CreatePatientRequest request);
        Task<bool> AddPatient(CreateEmergencyBurnPatientRequest request);
        Task<bool> DeletePatient(int patientId);
        Task<PatientResponse?> GetPatientById(int PatientId);
        Task<List<PatientResponse>?> GetPatients(PagedRequest request);
        Task<List<PatientSearchResponse>?> SearchPatient(SearchPatientRequest filters);
        Task<bool> UpdatePatient(UpdatePatientRequest request);
    }
}
