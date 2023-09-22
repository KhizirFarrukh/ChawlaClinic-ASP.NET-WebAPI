using ChawlaClinic.BL.DTOs.Patient;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity;
using System.Reflection;
using System.Transactions;

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

        }
        public (bool, string) UpdatePatient(AddPatientDTO dto)
        {

        }
        public bool DeletePatient(int Id)
        {

        }

        private string generateCaseNo(char type)
        {
            return "";
        }
    }
}
