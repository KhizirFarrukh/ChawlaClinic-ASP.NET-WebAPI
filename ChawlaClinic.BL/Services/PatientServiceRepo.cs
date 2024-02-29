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

namespace ChawlaClinic.BL.Services
{
    public class PatientServiceRepo : IPatientServiceRepo
    {
        ApplicationDbContext _dbContext;
        public PatientServiceRepo(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public List<PatientResponse>? GetPatients(PagedRequest request)
        {
            var patientDiscounts = _dbContext.DiscountOptions.ToList();

            var patients = _dbContext.Patients
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
                    Discount = patientDiscounts
                        .Where(y => y.DiscountId == x.DiscountId)
                        .Select(y => new DiscountResponse { DiscountId = y.DiscountId, Title = y.Title })
                        .FirstOrDefault(),
                })
                .ToList();

            return patients;
        }
        public PatientResponse? GetPatientById(int PatientId)
        {
            var patient = _dbContext.Patients
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
                .FirstOrDefault();

            return patient;
        }
        public List<PatientSearchResponse>? SearchPatient(string searchParam)
        {
            var patient_dicounts = _dbContext.DiscountOptions.ToList();
            var patients = _dbContext.Patients
                .Where(p =>
                     p.Name.Contains(searchParam, StringComparison.CurrentCultureIgnoreCase) ||
                    (p.PhoneNumber != null && p.PhoneNumber.Contains(searchParam, StringComparison.CurrentCultureIgnoreCase)) ||
                     p.CaseNo.Contains(searchParam, StringComparison.CurrentCultureIgnoreCase))
                .Select(p => new PatientSearchResponse
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    CaseNo = p.CaseNo,
                    Status = (PatientStatus)Enum.Parse(typeof(PatientStatus), x.Status),
                    FirstVisit = p.FirstVisit
                })
                .ToList();

            return patients;
        }
        public List<PatientSearchResponse>? SearchPatient(SearchPatientRequest filters)
        {
            var patient_dicounts = _dbContext.DiscountOptions.ToList();

            var patients = _dbContext.Patients
                .Where(p =>
                    (p.Name.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase) ||
                    (p.PhoneNumber != null && p.PhoneNumber.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase)) ||
                     p.CaseNo.Contains(filters.SearchParam, StringComparison.CurrentCultureIgnoreCase)) &&
                    (p.Type == ((char)filters.Type).ToString()) &&
                    (filters.FirstVisitStart == null || p.FirstVisit >= filters.FirstVisitStart) &&
                    (filters.FirstVisitEnd == null || p.FirstVisit <= filters.FirstVisitEnd) &&
                    (filters.ActiveStatus == FilterActiveStatus.All || p.IsActive == (filters.ActiveStatus == FilterActiveStatus.Active)) &&
                    (filters.DeleteStatus == FilterDeleteStatus.All || p.IsDeleted == (filters.DeleteStatus == FilterDeleteStatus.Deleted)))
                .Select(p => new PatientSearchResponse
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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _dbContext.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    if (dto.CaseNo == "") { dto.CaseNo = GenerateCaseNo(dto.Type); }

                    _dbContext.Patients.Add(new Patient
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

                    _dbContext.SaveChanges();

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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _dbContext.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    char type = 'B';
                    string caseNo = GenerateCaseNo(type);

                    _dbContext.Patients.Add(new Patient
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

                    _dbContext.SaveChanges();

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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var updateUserId = _dbContext.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (updateUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    var patient = _dbContext.Patients.Where(p => p.Id.ToString() == dto.Id).FirstOrDefault();

                    if (patient == null) { return (false, string.Format(CustomMessage.NOT_FOUND, "Patient")); }

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

                    _dbContext.SaveChanges();

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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var patient = _dbContext.Patients.Where(p => p.Id.ToString() == Id).FirstOrDefault();

                    if (patient == null) { return false; }

                    patient.IsActive = false;
                    patient.IsDeleted = true;

                    _dbContext.SaveChanges();

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
