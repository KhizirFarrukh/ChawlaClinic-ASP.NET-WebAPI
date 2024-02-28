using ChawlaClinic.BL.Requests.Patient;
using ChawlaClinic.BL.Responses.Patients;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        List<GetPatientResponse>? GetPatients();
        GetPatientResponse? GetPatientById(string Id);
        List<GetPatientForSearchDTO>? SearchPatient(string searchParam);
        List<GetPatientForSearchDTO>? SearchPatient(SearchPatientFiltersDTO filters);
        void AddPatient(CreatePatientRequest dto);
        void AddPatient(CreateEmergencyBurnPatientRequest dto);
        void AddPatient(IFormFile excelFile);
        (bool, string) UpdatePatient(UpdatePatientRequest dto);
        bool DeletePatient(string Id);
    }
}
