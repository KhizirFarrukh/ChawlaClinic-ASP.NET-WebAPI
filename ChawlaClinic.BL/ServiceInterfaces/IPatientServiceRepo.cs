using ChawlaClinic.BL.DTOs.Patient;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        List<GetPatientDTO>? GetPatients();
        GetPatientDTO? GetPatientById(string Id);
        List<GetPatientForSearchDTO>? SearchPatient(string searchParam);
        List<GetPatientForSearchDTO>? SearchPatient(SearchPatientFiltersDTO filters);
        void AddPatient(AddPatientDTO dto);
        void AddPatient(AddEmergencyBurnPatientDTO dto);
        void AddPatient(List<AddPatientDTO> bulkPatientDTOs);
        (bool, string) UpdatePatient(UpdatePatientDTO dto);
        bool DeletePatient(string Id);
    }
}
