﻿using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Exceptions;
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
            var sorting = request.GetSortingString();

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
                .OrderBy($"{request.SortColumn} {sorting}")
                .Skip(request.Page * request.Size)
                .Take(request.Size)
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

        public async Task<List<PatientSearchResponse>?> SearchPatient(SearchPatientRequest request)
        {
            var sorting = request.GetSortingString();

            var patients = await _dbContext.Patients
                .Where(x =>
                    (x.Name.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase) ||
                    (x.PhoneNumber != null && x.PhoneNumber.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase)) ||
                     x.CaseNo.Contains(request.SearchParam, StringComparison.CurrentCultureIgnoreCase)) &&
                    (request.Type == null || x.Type == ((char)request.Type).ToString()) &&
                    (request.FirstVisitStart == null || x.FirstVisit >= request.FirstVisitStart) &&
                    (request.FirstVisitEnd == null || x.FirstVisit <= request.FirstVisitEnd) &&
                    (request.Status == null || x.Status == request.Status.ToString()))
                .Select(x => new PatientSearchResponse
                {
                    PatientId = x.PatientId,
                    Name = x.Name,
                    CaseNo = x.CaseNo,
                    PhoneNumber = x.PhoneNumber,
                    Status = (PatientStatus)Enum.Parse(typeof(PatientStatus), x.Status),
                    FirstVisit = x.FirstVisit
                })
                .OrderBy($"{request.SortColumn} {sorting}")
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return patients;
        }

        public async Task<bool> AddPatient(CreatePatientRequest request)
        {
            var firstVisit = request.FirstVisit ?? DateOnly.FromDateTime(DateTime.Now);

            if ((request.CaseNo.Length == 4 && !int.TryParse(request.CaseNo, out _)) ||
                (!string.IsNullOrEmpty(request.CaseNo) && request.CaseNo.Length != 6))
                throw new ValidationFailedException("Invalid Case Number.");

            if (string.IsNullOrEmpty(request.CaseNo) || request.CaseNo.Length == 4)
                request.CaseNo = await GenerateCaseNo(request.Type, firstVisit, request.CaseNo);

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
                FirstVisit = firstVisit,
                Status = PatientStatus.Active.ToString(),
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPatient(CreateEmergencyBurnPatientRequest request)
        {
            var firstVisit = DateOnly.FromDateTime(DateTime.Now);

            string caseNo = await GenerateCaseNo(PatientType.Burns, firstVisit, null);

            await _dbContext.Patients.AddAsync(new Patient
            {
                Name = request.Name,
                GuardianName = request.GuardianName,
                AgeYears = request.AgeYears,
                AgeMonths = request.AgeMonths,
                Gender = ((char)request.Gender).ToString(),
                Type = ((char)PatientType.Burns).ToString(),
                Disease = null,
                Address = null,
                PhoneNumber = null,
                CaseNo = caseNo,
                FirstVisit = firstVisit,
                Status = PatientStatus.Active.ToString(),
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePatient(UpdatePatientRequest request)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == request.PatientId).FirstOrDefaultAsync();

            if (patient == null) throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            patient.Name = request.Name;
            patient.GuardianName = request.GuardianName;
            patient.AgeYears = request.AgeYears;
            patient.AgeMonths = request.AgeMonths;
            patient.Gender = ((char)request.Gender).ToString();
            patient.Disease = request.Disease;
            patient.Address = request.Address;
            patient.PhoneNumber = request.PhoneNumber;
            patient.FirstVisit = request.FirstVisit;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePatient(int patientId)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == patientId).FirstOrDefaultAsync();

            if (patient == null) throw new NotFoundException($"Patient with ID {patientId} was not found.");

            _dbContext.Patients.Remove(patient);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        private async Task<string> GenerateCaseNo(PatientType type, DateOnly firstVisit, string? caseNo)
        {
            var newCaseNo = firstVisit.Year.ToString().Substring(2) + ((char)type) + '-';

            if (string.IsNullOrEmpty(caseNo))
            {
                var caseNoMax = await _dbContext.Patients
                    .Where(x => x.CaseNo.StartsWith(newCaseNo))
                    .Select(x => int.Parse(x.CaseNo.Substring(x.CaseNo.Length - 6)))
                    .MaxAsync();

                newCaseNo += (caseNoMax + 1).ToString();
            }
            else if (caseNo.Length == 4)
            {
                var caseNoMax = await _dbContext.Patients
                    .Where(x => x.CaseNo.StartsWith(newCaseNo) && x.CaseNo.EndsWith(caseNo))
                    .Select(x => int.Parse(x.CaseNo.Substring(x.CaseNo.Length - 6, 2)))
                    .MaxAsync();

                if (caseNoMax == 99)
                    throw new BadRequestException("Already maximum number of case numbers exists for the last 4 digits of token number provided.");

                newCaseNo = newCaseNo + (caseNoMax + 1).ToString("D2") + caseNo;
            }

            return newCaseNo;
        }
    }
}
