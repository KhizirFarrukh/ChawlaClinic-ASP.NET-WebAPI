using ChawlaClinic.BL.DTOs.Patient;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPatientServiceRepo
    {
        List<GetPatientDTO>? GetPatients();
        GetPatientDTO? GetPatientById(string Id);
        List<GetPatientForSearchDTO>? SearchPatient(string searchParam);
        void AddPatient(AddPatientDTO dto);
        void AddPatient(AddEmergencyBurnPatientDTO dto);
        (bool, string) UpdatePatient(AddPatientDTO dto);
        bool DeletePatient(int Id);
    }
}
