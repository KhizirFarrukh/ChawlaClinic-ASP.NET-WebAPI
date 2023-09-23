using ChawlaClinic.API.Models;
using ChawlaClinic.BL.DTOs.Patient;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChawlaClinic.API.Controllers
{
    public class PatientController : ControllerBase
    {
        private IPatientServiceRepo _patientRepo;
        public PatientController(IPatientServiceRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        [HttpGet("GetPatients")]
        public IActionResult GetPatients()
        {
            try
            {
                var patients = _patientRepo.GetPatients();
                return Ok(new JSONResponse { Status = true, Data = patients });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPatientById")]
        public IActionResult GetPatientById(string Id)
        {
            try
            {
                var patient = _patientRepo.GetPatientById(Id);
                string message = "";
                bool status = true;

                if(patient == null)
                {
                    message = string.Format(CustomMessage.NOT_FOUND, "Patient");
                }

                return Ok(new JSONResponse { Status = status, Data = patient, Message = message });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("SearchPatient")]
        public IActionResult SearchPatient(string searchParam)
        {
            try
            {
                var patients = _patientRepo.SearchPatient(searchParam);

                return Ok(new JSONResponse { Status = true, Data = patients });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("SearchPatientWithFilters")]
        public IActionResult SearchPatientWithFilters(SearchPatientFiltersDTO filters)
        {
            try
            {
                var patients = _patientRepo.SearchPatient(filters);

                return Ok(new JSONResponse { Status = true, Data = patients });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = false, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
