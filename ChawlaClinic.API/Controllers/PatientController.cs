using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
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
        public async Task<IActionResult> GetPatients([FromQuery] int? size, int? page, bool? isAscending, string? sortColumn)
        {
            try
            {
                return Ok(await _patientRepo.GetPatients(new PagedRequest(size: size, page: page, isAscending: isAscending, sortColumn: sortColumn ?? "CaseNo")));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("GetPatientById")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            try
            {
                return Ok(await _patientRepo.GetPatientById(patientId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("SearchPatient")]
        public async Task<IActionResult> SearchPatient(SearchPatientRequest request)
        {
            try
            {
                return Ok(await _patientRepo.SearchPatient(request));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("AddPatient")]
        public async Task<IActionResult> AddPatient([FromBody] CreatePatientRequest request)
        {
            try
            {
                return Ok(await _patientRepo.AddPatient(request));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("AddEmergencyBurnPatient")]
        public async Task<IActionResult> AddEmergencyBurnPatient([FromBody] CreateEmergencyBurnPatientRequest request)
        {
            try
            {
                return Ok(await _patientRepo.AddPatient(request));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
