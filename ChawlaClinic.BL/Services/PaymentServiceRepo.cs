using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Helpers;
using ChawlaClinic.Common.Requests.Payment;
using ChawlaClinic.Common.Responses.Discounts;
using ChawlaClinic.Common.Responses.Payments;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using System.Data.Entity;
using System.Linq.Dynamic.Core;

namespace ChawlaClinic.BL.Services
{
    public class PaymentServiceRepo : IPaymentServiceRepo
    {
        private ApplicationDbContext _dbContext;
        public PaymentServiceRepo(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> AddPayment(CreatePaymentRequest request)
        {
            var patient = await _dbContext.Patients.Where(x => x.PatientId == request.PatientId).FirstOrDefaultAsync();

            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            //var discount = await _dbContext.DiscountOptions.Where(x => x.DiscountId == request.DiscountId).FirstOrDefaultAsync();

            //if (discount == null)
            //    throw new NotFoundException($"Discount with ID {request.DiscountId} was not found.");

            var payment = new Payment
            {
                AmountPaid = request.AmountPaid,
                DateTime = request.DateTime ?? DateTime.Now,
                PatientId = patient.PatientId,
                DiscountId = patient.DiscountId
            };

            payment.SecureToken = CommonHelper.GenerateSecureToken(payment.TokenID);

            await _dbContext.Payments.AddAsync(payment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<GetPaymentResponse>?> GetPaymentsByPatientId(GetPaymentsByPatientIdRequest request)
        {
            var patient = await _dbContext.Patients
                .Where(x => x.PatientId == request.PatientId)
                .FirstOrDefaultAsync();

            if (patient == null)
                throw new NotFoundException($"Patient with ID {request.PatientId} was not found.");

            var sorting = request.GetSortingString();

            var payments = await _dbContext.Payments
                .Include(x => x.Discount)
                .Where(x => x.PatientId == patient.PatientId)
                .Select(x => new GetPaymentResponse
                {
                    PaymentId = x.PaymentId,
                    AmountPaid = x.AmountPaid,
                    DateTime = x.DateTime,
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

            return payments;
        }

        public async Task<GetPaymentResponse?> GetPaymentByPaymentId(int paymentId)
        {
            return await _dbContext.Payments
                .Include(x => x.Discount)
                .Where(x => x.PaymentId == paymentId)
                .Select(x => new GetPaymentResponse
                {
                    PaymentId = x.PaymentId,
                    AmountPaid = x.AmountPaid,
                    DateTime = x.DateTime,
                    Discount = new DiscountResponse
                    {
                        DiscountId = x.Discount.DiscountId,
                        Title = x.Discount.Title
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePayment(UpdatePaymentRequest request)
        {
            var discount = await _dbContext.DiscountOptions.Where(x => x.DiscountId == request.DiscountId).FirstOrDefaultAsync();

            if (discount == null)
                throw new NotFoundException($"Discount with ID {request.DiscountId} was not found.");

            var payment = await _dbContext.Payments.Where(x => x.PaymentId == request.PaymentId).FirstOrDefaultAsync();

            if (payment == null) 
                throw new NotFoundException($"Payment with ID {request.PaymentId} was not found.");

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

            _dbContext.Payments.Remove(payment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}