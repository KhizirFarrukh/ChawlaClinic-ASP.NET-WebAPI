using ChawlaClinic.BL.DTOs.Patient;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity;
using System.Reflection;
using System.Transactions;
using System.Xml.Linq;

namespace ChawlaClinic.BL.Services
{
    public class PatientServiceRepo : IPatientServiceRepo
    {
        ApplicationDbContext _context;
        public PatientServiceRepo(ApplicationDbContext context) 
        {
            _context = context;
        }
        public List<GetPatientDTO>? GetPatients()
        {
            var patient_dicounts = _context.PatientDiscounts.ToList();
            var patients = _context.Patients
                .Where(p => p.IsDeleted == false)
                .Select(p => new GetPatientDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    GuardianName = p.GuardianName,
                    AgeYears = p.AgeYears,
                    AgeMonths = p.AgeMonths,
                    Gender = p.Gender,
                    Type = p.Type,
                    Disease = p.Disease,
                    Address = p.Address,
                    PhoneNumber = p.PhoneNumber,
                    CaseNo = p.CaseNo,
                    Status = p.IsActive,
                    FirstVisit = p.FirstVisit,
                    DiscountId = p.DiscountId,
                    Discount = patient_dicounts.Where(pd => pd.Id == p.DiscountId).Select(pd => pd.Title).FirstOrDefault() ?? ""
                })
                .ToList();

            return patients;
        }
        public GetPatientDTO? GetPatientById(string Id)
        {
            var patient_dicounts = _context.PatientDiscounts.ToList();
            var patient = _context.Patients
                .Where(p => 
                    p.Id.ToString() == Id &&
                    p.IsDeleted == false)
                .Select(p => new GetPatientDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    GuardianName = p.GuardianName,
                    AgeYears = p.AgeYears,
                    AgeMonths = p.AgeMonths,
                    Gender = p.Gender,
                    Type = p.Type,
                    Disease = p.Disease,
                    Address = p.Address,
                    PhoneNumber = p.PhoneNumber,
                    CaseNo = p.CaseNo,
                    Status = p.IsActive,
                    FirstVisit = p.FirstVisit,
                    DiscountId = p.DiscountId,
                    Discount = patient_dicounts.Where(pd => pd.Id == p.DiscountId).Select(pd => pd.Title).FirstOrDefault() ?? ""
                })
                .FirstOrDefault();

            return patient;
        }
        public List<GetPatientForSearchDTO>? SearchPatient(string searchParam)
        {
            searchParam = searchParam.ToUpper();

            var patient_dicounts = _context.PatientDiscounts.ToList();
            var patients = _context.Patients
                .Where(p =>
                    (p.Name.ToUpper().Contains(searchParam) ||
                        p.PhoneNumber.ToUpper().Contains(searchParam) ||
                        p.CaseNo.ToUpper().Contains(searchParam)) &&
                    p.IsDeleted == false)
                .Select(p => new GetPatientForSearchDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    CaseNo = p.CaseNo,
                    Status = p.IsActive,
                    FirstVisit = p.FirstVisit
                })
                .ToList();

            return patients;
        }
        public List<GetPatientForSearchDTO>? SearchPatient(SearchPatientFiltersDTO filters)
        {
            filters.SearchParam = filters.SearchParam.ToUpper();

            var patient_dicounts = _context.PatientDiscounts.ToList();
            
            var patients = _context.Patients
                .Where(p =>
                    (p.Name.ToUpper().Contains(filters.SearchParam) ||
                     p.PhoneNumber.ToUpper().Contains(filters.SearchParam) ||
                     p.CaseNo.ToUpper().Contains(filters.SearchParam)) &&
                    (p.Type == filters.Type) &&
                    (filters.FirstVisitStart == null || p.FirstVisit >= filters.FirstVisitStart) &&
                    (filters.FirstVisitEnd == null || p.FirstVisit <= filters.FirstVisitEnd) &&
                    (filters.ActiveStatus == FilterActiveStatus.All || p.IsActive == (filters.ActiveStatus == FilterActiveStatus.Active)) &&
                    (filters.DeleteStatus == FilterDeleteStatus.All || p.IsDeleted == (filters.DeleteStatus == FilterDeleteStatus.Deleted)))
                .Select(p => new GetPatientForSearchDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    CaseNo = p.CaseNo,
                    Status = p.IsActive,
                    FirstVisit = p.FirstVisit
                })
                .Skip(filters.PageNumber * filters.PageSize)
                .Take(filters.PageSize)
                .ToList();

            return patients;
        }
        public void AddPatient(AddPatientDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    if (dto.CaseNo == "") { dto.CaseNo = generateCaseNo(dto.Type); }

                    _context.Patients.Add(new Patient
                    {
                        Name = dto.Name,
                        GuardianName = dto.GuardianName,
                        AgeYears = dto.AgeYears,
                        AgeMonths = dto.AgeMonths,
                        Gender = dto.Gender,
                        Type = dto.Type,
                        Disease = dto.Disease,
                        Address = dto.Address,
                        PhoneNumber = dto.PhoneNumber,
                        CaseNo = dto.CaseNo,
                        FirstVisit = dto.FirstVisit,
                        IsActive = true,
                        IsDeleted = false,
                        AddedOn = DateTime.Now,
                        ModifiedOn = null,
                        AddedBy = addUserId ?? -1,
                        ModifiedBy = null
                    });

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }
        public void AddPatient(AddEmergencyBurnPatientDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    char type = 'B';
                    string caseNo = generateCaseNo(type);

                    _context.Patients.Add(new Patient
                    {
                        Name = dto.Name,
                        GuardianName = dto.GuardianName,
                        AgeYears = dto.AgeYears,
                        AgeMonths = dto.AgeMonths,
                        Gender = dto.Gender,
                        Type = type,
                        Disease = "",
                        Address = "",
                        PhoneNumber = "",
                        CaseNo = caseNo,
                        FirstVisit = DateOnly.FromDateTime(DateTime.Now),
                        IsActive = true,
                        IsDeleted = false,
                        AddedOn = DateTime.Now,
                        ModifiedOn = null,
                        AddedBy = addUserId ?? -1,
                        ModifiedBy = null
                    });

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }
        public void AddPatient(List<AddPatientDTO> bulkPatientDTOs)
        {

        }
        public (bool, string) UpdatePatient(UpdatePatientDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var updateUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (updateUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    var patient = _context.Patients.Where(p => p.Id.ToString() == dto.Id).FirstOrDefault();

                    if(patient == null) { return (false,  string.Format(CustomMessage.NOT_FOUND, "Patient")); }

                    if (dto.CaseNo == "") { dto.CaseNo = generateCaseNo(dto.Type); }

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

                    _context.SaveChanges();

                    transaction.Commit();

                    return (true, string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Patient"));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }
        public bool DeletePatient(string Id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var patient = _context.Patients.Where(p => p.Id.ToString() == Id).FirstOrDefault();

                    if (patient == null) { return false; }

                    patient.IsActive = false;
                    patient.IsDeleted = true;

                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }

        private string generateCaseNo(char type)
        {
            return "";
        }
    }
}
