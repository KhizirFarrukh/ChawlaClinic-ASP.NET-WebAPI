using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Helpers;
using ChawlaClinic.Common.Requests.Payment;
using ChawlaClinic.Common.Responses.Commons;
using ChawlaClinic.Common.Responses.Discounts;
using ChawlaClinic.Common.Responses.Payments;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ChawlaClinic.BL.Services
{
    public class PaymentServiceRepo(ApplicationDbContext dbContext) : BaseServiceRepo<Payment>(dbContext), IPaymentServiceRepo
    {
        public async Task<bool> AddPayment(CreatePaymentRequest request)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == request.PatientId).FirstOrDefaultAsync();

            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            var paymentId = await GetSequence();

            var payment = new Payment
            {
                PaymentId = paymentId,
                AmountPaid = request.AmountPaid,
                Code = CommonHelper.GenerateSecureCode(paymentId),
                DateTime = request.DateTime ?? DateTime.Now,
                PatientId = patient.PatientId,
                DiscountId = patient.DiscountId,
            };

            await _dbContext.Payments.AddAsync(payment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<PaginatedList<PaymentResponse>> GetPaymentsByPatientId(GetPaymentsByPatientIdRequest request)
        {
            var patient = await _dbContext.Patients
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId);

            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            if (patient.Status == PatientStatus.Deleted.ToString())
                throw new BadRequestException($"Patient with ID {request.PatientId} is deleted.");

            var sorting = request.GetSortingString();

            var query = _dbContext.Payments
                .Include(x => x.Discount)
                .Where(x => x.PatientId == patient.PatientId &&
                    x.Status != PaymentStatus.Deleted.ToString());

            var totalCount = await query.CountAsync();

            var payments = await query
                .Select(x => new PaymentResponse
                {
                    PaymentId = x.PaymentId,
                    Code = x.Code,
                    AmountPaid = x.AmountPaid,
                    DateTime = x.DateTime,
                    Status = x.Status,
                    Discount = new DiscountResponse
                    {
                        DiscountId = x.Discount.DiscountId,
                        Title = x.Discount.Title
                    }
                })
                .OrderBy($"{request.SortColumn} {sorting}")
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .ToListAsync();

            return new PaginatedList<PaymentResponse>(payments, totalCount, request.Page, request.Size);
        }

        public async Task<PaymentResponse?> GetPaymentByPaymentId(int paymentId)
        {
            var payment = await _dbContext.Payments
                .Include(x => x.Discount)
                .Where(x => x.PaymentId == paymentId)
                .Select(x => new PaymentResponse
                {
                    PaymentId = x.PaymentId,
                    Code = x.Code,
                    AmountPaid = x.AmountPaid,
                    DateTime = x.DateTime,
                    Status = x.Status,
                    Discount = new DiscountResponse
                    {
                        DiscountId = x.Discount.DiscountId,
                        Title = x.Discount.Title
                    }
                })
                .FirstOrDefaultAsync();

            if (payment != null && payment.Status == PaymentStatus.Deleted.ToString())
                throw new BadRequestException($"Payment with ID {paymentId} is deleted.");

            return payment;
        }

        public async Task<PaymentResponse?> GetPaymentByPaymentCode(string paymentCode)
        {
            var payment = await _dbContext.Payments
                .Include(x => x.Discount)
                .Where(x => x.Code == paymentCode)
                .Select(x => new PaymentResponse
                {
                    PaymentId = x.PaymentId,
                    Code = x.Code,
                    AmountPaid = x.AmountPaid,
                    DateTime = x.DateTime,
                    Status = x.Status,
                    Discount = new DiscountResponse
                    {
                        DiscountId = x.Discount.DiscountId,
                        Title = x.Discount.Title
                    }
                })
                .FirstOrDefaultAsync();

            if (payment != null && payment.Status == PaymentStatus.Deleted.ToString())
                throw new BadRequestException($"Payment with code {paymentCode} is deleted.");

            return payment;
        }

        public async Task<bool> UpdatePayment(UpdatePaymentRequest request)
        {
            var discount = await _dbContext.DiscountOptions.Where(x => x.DiscountId == request.DiscountId).FirstOrDefaultAsync();

            if (discount == null)
                throw new NotFoundException($"Discount with ID {request.DiscountId} was not found.");

            var payment = await _dbContext.Payments.Where(x => x.PaymentId == request.PaymentId).FirstOrDefaultAsync();

            if (payment == null)
                throw new NotFoundException($"Payment with ID {request.PaymentId} was not found.");

            if (payment.Status == PaymentStatus.Deleted.ToString())
                throw new BadRequestException($"Payment with ID {request.PaymentId} is deleted.");

            payment.AmountPaid = request.AmountPaid;
            payment.DateTime = request.DateTime;
            payment.DiscountId = request.DiscountId;
            payment.Discount = discount;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePayment(int paymentId)
        {
            var payment = await _dbContext.Payments.Where(x => x.PaymentId == paymentId).FirstOrDefaultAsync();

            if (payment == null)
                throw new NotFoundException($"Payment with ID {paymentId} was not found.");

            payment.Status = PaymentStatus.Deleted.ToString();

            _dbContext.Payments.Update(payment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
