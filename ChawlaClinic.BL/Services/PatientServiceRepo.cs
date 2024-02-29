using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Discounts;
using ChawlaClinic.Common.Responses.Patients;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ChawlaClinic.BL.Services
{
    public class PatientServiceRepo : IPatientServiceRepo
    {
        ApplicationDbContext _dbContext;

        public PatientServiceRepo(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<PatientResponse>?> GetPatients(PagedRequest request)
        {
            var sorting = request.IsAscending ? "ascending" : "descending";

            var patients = await _dbContext.Patients
                .Include(x => x.Discount)
                .Select(x => new PatientResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    Description = x.Description,
                    GuardianName = x.GuardianName,
                    AgeYears = x.AgeYears,
                    AgeMonths = x.AgeMonths,
                    Gender = x.Gender,
                    Type = (PatientType)Enum.Parse(typeof(PatientType), x.Type),
                    Disease = x.Disease,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CaseNo = x.CaseNo,
                    Status = (PatientStatus)Enum.Parse(typeof(PatientStatus), x.Status),
                    FirstVisit = x.FirstVisit,
                    Discount = x.Discount == null ? null :
                        new DiscountResponse
                        {
                            DiscountId = x.Discount.DiscountId,
                            Title = x.Discount.Title
                        }
                })
                .ToListAsync();

            return patients;
        }

        public async Task<PatientResponse?> GetPatientById(int PatientId)
        {
            var patient = await _dbContext.Patients
                .Include(x => x.Discount)
                .Where(x => x.PatientId == PatientId)
                .Select(x => new PatientResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    Description = x.Description,
                    GuardianName = x.GuardianName,
                    AgeYears = x.AgeYears,
                    AgeMonths = x.AgeMonths,
                    Gender = x.Gender,
                    Type = (PatientType)Enum.Parse(typeof(PatientType), x.Type),
                    Disease = x.Disease,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CaseNo = x.CaseNo,
                    Status = (PatientStatus)Enum.Parse(typeof(PatientStatus), x.Status),
                    FirstVisit = x.FirstVisit,
                    Discount = x.Discount == null ? null :
                        new DiscountResponse
                        {
                            DiscountId = x.Discount.DiscountId,
                            Title = x.Discount.Title
                        }
                })
                .FirstOrDefaultAsync();

            return patient;
        }

        public async Task<List<PatientSearchResponse>?> SearchPatient(SearchPatientRequest filters)
        {
            var sorting = filters.IsAscending ? "ascending" : "descending";

            var patients = await _dbContext.Patients
                .Where(x =>
                    (x.Name.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase) ||
                    (x.PhoneNumber != null && x.PhoneNumber.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase)) ||
                     x.CaseNo.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase)) &&
                    (filters.Type == null || x.Type == ((char)filters.Type).ToString()) &&
                    (filters.FirstVisitStart == null || x.FirstVisit >= filters.FirstVisitStart) &&
                    (filters.FirstVisitEnd == null || x.FirstVisit <= filters.FirstVisitEnd) &&
                    (filters.Status == null || x.Status == filters.Status.ToString()))
                .Select(x => new PatientSearchResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    CaseNo = x.CaseNo,
                    Status = (PatientStatus)Enum.Parse(typeof(PatientStatus), x.Status),
                    FirstVisit = x.FirstVisit
                })
                .OrderBy($"{filters.SortColumn} {sorting}")
                .Skip(filters.Page * filters.Size)
                .Take(filters.Size)
                .ToListAsync();

            return patients;
        }

        public async Task<bool> AddPatient(CreatePatientRequest request)
        {
            if (request.CaseNo == "") { request.CaseNo = await GenerateCaseNo(request.Type); }

            await _dbContext.Patients.AddAsync(new Patient
            {
                Name = request.Name,
                GuardianName = request.GuardianName,
                AgeYears = request.AgeYears,
                AgeMonths = request.AgeMonths,
                Gender = ((char)request.Gender).ToString(),
                Type = ((char)request.Type).ToString(),
                Disease = request.Disease,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                CaseNo = request.CaseNo,
                FirstVisit = request.FirstVisit,
                Status = PatientStatus.Active.ToString(),
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPatient(CreateEmergencyBurnPatientRequest dto)
        {
            string caseNo = await GenerateCaseNo(PatientType.Burns);

            await _dbContext.Patients.AddAsync(new Patient
            {
                Name = dto.Name,
                GuardianName = dto.GuardianName,
                AgeYears = dto.AgeYears,
                AgeMonths = dto.AgeMonths,
                Gender = ((char)dto.Gender).ToString(),
                Type = ((char)PatientType.Burns).ToString(),
                Disease = null,
                Address = null,
                PhoneNumber = null,
                CaseNo = caseNo,
                FirstVisit = DateOnly.FromDateTime(DateTime.Now),
                Status = PatientStatus.Active.ToString(),
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePatient(UpdatePatientRequest dto)
        {
            var patient = await _dbContext.Patients.Where(p => p.Id.ToString() == dto.Id).FirstOrDefaultAsync();

            if (patient == null) { return (false, string.Format(CustomMessage.NOT_FOUND, "Patient")); }

            if (dto.CaseNo == "") { dto.CaseNo = await GenerateCaseNo(dto.Type); }

            patient.Name = dto.Name;
            patient.GuardianName = dto.GuardianName;
            patient.AgeYears = dto.AgeYears;
            patient.AgeMonths = dto.AgeMonths;
            patient.Gender = dto.Gender;
            patient.Disease = dto.Disease;
            patient.Address = dto.Address;
            patient.PhoneNumber = dto.PhoneNumber;
            patient.FirstVisit = dto.FirstVisit;
            patient.ModifiedOn = DateTime.Now;
            patient.ModifiedBy = updateUserId ?? -1;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePatient(int patientId)
        {
            var patient = await _dbContext.Patients.Where(p => p.PatientId == patientId).FirstOrDefaultAsync();

            if (patient == null) { return false; }

            _dbContext.Patients.Remove(patient);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        private async Task<string> GenerateCaseNo(PatientType type)
        {
            return "";
        }
    }
}
