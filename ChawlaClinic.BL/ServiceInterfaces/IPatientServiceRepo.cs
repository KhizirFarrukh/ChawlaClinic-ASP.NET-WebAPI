using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Patients;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        List<PatientResponse>? GetPatients();
        PatientResponse? GetPatientById(string Id);
        List<GetPatientForSearchDTO>? SearchPatient(string searchParam);
        List<GetPatientForSearchDTO>? SearchPatient(SearchPatientFiltersDTO filters);
        void AddPatient(CreatePatientRequest dto);
        void AddPatient(CreateEmergencyBurnPatientRequest dto);
        void AddPatient(IFormFile excelFile);
        (bool, string) UpdatePatient(UpdatePatientRequest dto);
        bool DeletePatient(string Id);
    }
}
