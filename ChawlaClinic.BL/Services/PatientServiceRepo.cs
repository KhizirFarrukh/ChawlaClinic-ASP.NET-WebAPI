﻿using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Commons;
using ChawlaClinic.Common.Responses.Discounts;
using ChawlaClinic.Common.Responses.Patients;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ChawlaClinic.BL.Services
{
    public class PatientServiceRepo : BaseServiceRepo<Patient>, IPatientServiceRepo
    {
        public PatientServiceRepo(ApplicationDbContext dbContext) : base(dbContext)
        { }

        public async Task<PaginatedList<PatientResponse>> GetPatients(PagedRequest request)
        {
            var sorting = request.GetSortingString();

            var query = _dbContext.Patients
                .AsNoTracking()
                .Include(x => x.Discount)
                .Where(x => x.Status != PatientStatus.Deleted.ToString());

            var totalCount = await query.CountAsync();

            var patients = await query
                .Select(x => new PatientResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    Description = x.Description,
                    GuardianName = x.GuardianName,
                    AgeYears = x.AgeYears,
                    AgeMonths = x.AgeMonths,
                    Gender = x.Gender,
                    Type = x.Type[0],
                    Disease = x.Disease,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CaseNo = x.CaseNo,
                    Status = x.Status,
                    FirstVisit = x.FirstVisit,
                    Discount = x.Discount == null ? null :
                        new DiscountResponse
                        {
                            DiscountId = x.Discount.DiscountId,
                            Title = x.Discount.Title
                        }
                })
                .OrderBy($"{request.SortColumn} {sorting}")
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return new PaginatedList<PatientResponse>(patients, totalCount, request.Page, request.Size);
        }

        public async Task<PatientResponse?> GetPatientById(int patientId)
        {
            var patient = await _dbContext.Patients
                .Include(x => x.Discount)
                .Where(x => x.PatientId == patientId)
                .Select(x => new PatientResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    Description = x.Description,
                    GuardianName = x.GuardianName,
                    AgeYears = x.AgeYears,
                    AgeMonths = x.AgeMonths,
                    Gender = x.Gender,
                    Type = x.Type[0],
                    Disease = x.Disease,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CaseNo = x.CaseNo,
                    Status = x.Status,
                    FirstVisit = x.FirstVisit,
                    Discount = x.Discount == null ? null :
                        new DiscountResponse
                        {
                            DiscountId = x.Discount.DiscountId,
                            Title = x.Discount.Title
                        }
                })
                .FirstOrDefaultAsync();

            if (patient != null && patient.Status == PatientStatus.Deleted.ToString())
                throw new BadRequestException($"Patient with ID {patientId} is deleted.");

            return patient;
        }

        public async Task<PaginatedList<PatientSearchResponse>> SearchPatient(SearchPatientRequest request)
        {
            var sorting = request.GetSortingString();

            var query = _dbContext.Patients
                .Where(x =>
                    x.Status != PatientStatus.Deleted.ToString() &&
                    (x.Name.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase) ||
                    (x.PhoneNumber != null && x.PhoneNumber.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase)) ||
                     x.CaseNo.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase)) &&
                    (request.Type == null || x.Type == ((char)request.Type).ToString()) &&
                    (request.FirstVisitStart == null || x.FirstVisit >= request.FirstVisitStart) &&
                    (request.FirstVisitEnd == null || x.FirstVisit <= request.FirstVisitEnd) &&
                    (request.Status == null || x.Status == request.Status.ToString()));

            var totalCount = await query.CountAsync();

            var patients = await query
                .Select(x => new PatientSearchResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    CaseNo = x.CaseNo,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    FirstVisit = x.FirstVisit
                })
                .OrderBy($"{request.SortColumn} {sorting}")
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return new PaginatedList<PatientSearchResponse>(patients, totalCount, request.Page, request.Size);
        }

        public async Task<bool> AddPatient(CreatePatientRequest request)
        {
            var firstVisit = request.FirstVisit ?? DateTime.Now;

            if (request.CaseNo != null && ((request.CaseNo.Length == 4 && !int.TryParse(request.CaseNo, out _)) || request.CaseNo.Length != 4))
                throw new ValidationFailedException("Invalid Case Number.");

            if (string.IsNullOrEmpty(request.CaseNo) || request.CaseNo.Length == 4)
                request.CaseNo = await GenerateCaseNo(request.Type, firstVisit, request.CaseNo);

            var defaultDiscount = await _dbContext.DiscountOptions.FirstAsync(x => x.DiscountId == 1 || x.Title == "None");

            var patientId = await GetSequence();

            await _dbContext.Patients.AddAsync(new Patient
            {
                PatientId = patientId,
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
                FirstVisit = firstVisit,
                Status = PatientStatus.Active.ToString(),
                Description = null,
                DiscountId = defaultDiscount.DiscountId
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPatient(CreateEmergencyBurnPatientRequest request)
        {
            var firstVisit = DateTime.Now;

            string caseNo = await GenerateCaseNo('B', firstVisit, null);

            var defaultDiscount = await _dbContext.DiscountOptions.FirstAsync(x => x.DiscountId == 1 || x.Title == "None");

            var patientId = await GetSequence();

            await _dbContext.Patients.AddAsync(new Patient
            {
                PatientId = patientId,
                Name = request.Name,
                GuardianName = request.GuardianName,
                AgeYears = request.AgeYears,
                AgeMonths = request.AgeMonths,
                Gender = request.Gender.ToString(),
                Type = 'B'.ToString(),
                Disease = null,
                Address = null,
                PhoneNumber = null,
                CaseNo = caseNo,
                FirstVisit = firstVisit,
                Status = PatientStatus.Active.ToString(),
                DiscountId = defaultDiscount.DiscountId,
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePatient(UpdatePatientRequest request)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == request.PatientId).FirstOrDefaultAsync()
                ?? throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            if (patient.Status == PatientStatus.Deleted.ToString())
                throw new BadRequestException($"Patient with ID {request.PatientId} is deleted.");

            patient.Name = request.Name;
            patient.GuardianName = request.GuardianName;
            patient.AgeYears = request.AgeYears;
            patient.AgeMonths = request.AgeMonths;
            patient.Gender = request.Gender.ToString();
            patient.Disease = request.Disease;
            patient.Address = request.Address;
            patient.PhoneNumber = request.PhoneNumber;
            patient.FirstVisit = request.FirstVisit;
            patient.Status = request.Status;
            patient.DiscountId = request.DiscountId;
            patient.Discount = await _dbContext.DiscountOptions.FirstAsync(x => x.DiscountId == request.DiscountId);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePatient(int patientId)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == patientId).FirstOrDefaultAsync();

            if (patient == null) throw new NotFoundException($"Patient with ID {patientId} was not found.");

            patient.Status = PatientStatus.Deleted.ToString(); // add this new status to check condition in tables

            _dbContext.Patients.Update(patient);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        private async Task<string> GenerateCaseNo(char type, DateTime firstVisit, string? caseNo)
        {
            var newCaseNo = firstVisit.Year.ToString().Substring(2) + type + '-';

            if (string.IsNullOrEmpty(caseNo))
            {
                var matchingCaseNos = await _dbContext.Patients
                    .Where(x => x.CaseNo.StartsWith(newCaseNo))
                    .Select(x => x.CaseNo.Substring(x.CaseNo.Length - 6))
                    .ToListAsync();

                var caseNoMax = matchingCaseNos
                    .Select(x => int.Parse(x))
                    .Max();

                newCaseNo += (caseNoMax + 1).ToString().PadLeft(6, '0');
            }
            else if (caseNo.Length == 4)
            {
                var caseNoNext = _dbContext.Patients
                    .Where(x => x.CaseNo.StartsWith(newCaseNo) && x.CaseNo.EndsWith(caseNo))
                    .Select(x => int.Parse(x.CaseNo.Substring(x.CaseNo.Length - 6, 2)) + 1)
                    .AsEnumerable()
                    .DefaultIfEmpty(0)
                    .Max();

                if (caseNoNext == 99)
                    throw new BadRequestException("Already maximum number of case numbers exists for the last 4 digits of token number provided.");

                newCaseNo = newCaseNo + caseNoNext.ToString("D2") + caseNo;
            }

            return newCaseNo;
        }
    }
}
