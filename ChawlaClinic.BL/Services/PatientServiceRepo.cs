﻿using ChawlaClinic.BL.Requests.Patient;
using ChawlaClinic.BL.Responses.Patients;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
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
        public List<GetPatientResponse>? GetPatients()
        {
            var patient_dicounts = _context.PatientDiscounts.ToList();
            var patients = _context.Patients
                .Where(p => p.IsDeleted == false)
                .Select(p => new GetPatientResponse
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
        public GetPatientResponse? GetPatientById(string Id)
        {
            var patient_dicounts = _context.PatientDiscounts.ToList();
            var patient = _context.Patients
                .Where(p => 
                    p.Id.ToString() == Id &&
                    p.IsDeleted == false)
                .Select(p => new GetPatientResponse
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
        public void AddPatient(CreatePatientRequest dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    if (dto.CaseNo == "") { dto.CaseNo = GenerateCaseNo(dto.Type); }

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
        public void AddPatient(CreateEmergencyBurnPatientRequest dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    char type = 'B';
                    string caseNo = GenerateCaseNo(type);

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
        public void AddPatient(IFormFile excelFile)
        {
            var patientDtos = ParseExcelFile(excelFile);
        }
        public (bool, string) UpdatePatient(UpdatePatientRequest dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var updateUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (updateUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    var patient = _context.Patients.Where(p => p.Id.ToString() == dto.Id).FirstOrDefault();

                    if(patient == null) { return (false, string.Format(CustomMessage.NOT_FOUND, "Patient")); }

                    if (dto.CaseNo == "") { dto.CaseNo = GenerateCaseNo(dto.Type); }

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

        private string GenerateCaseNo(char type)
        {
            return "";
        }
        private List<CreatePatientRequest>? ParseExcelFile(IFormFile excelFile)
        {
            var patients = new List<CreatePatientRequest>();

            using (var stream = excelFile.OpenReadStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    int startRow = 2;

                    for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var patient = new CreatePatientRequest
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            GuardianName = worksheet.Cells[row, 2].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            AgeYears = Convert.ToInt32(worksheet.Cells[row, 3].Value),
                            AgeMonths = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                            Gender = worksheet.Cells[row, 5].Value?.ToString()[0] ?? throw new Exception("Error parsing Excel file"),
                            Type = worksheet.Cells[row, 6].Value?.ToString()[0] ?? throw new Exception("Error parsing Excel file"),
                            Disease = worksheet.Cells[row, 7].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            Address = worksheet.Cells[row, 8].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            PhoneNumber = worksheet.Cells[row, 9].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            CaseNo = worksheet.Cells[row, 10].Value?.ToString() ?? throw new Exception("Error parsing Excel file"),
                            FirstVisit = DateOnly.Parse(worksheet.Cells[row, 11].Value?.ToString() ?? throw new Exception("Error parsing Excel file"))
                        };

                        patients.Add(patient);
                    }
                }
            }

            return patients;
        }
    }
}
