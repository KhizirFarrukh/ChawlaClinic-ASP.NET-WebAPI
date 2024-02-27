using ChawlaClinic.BL.Requests.Payment;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Commons;
using ChawlaClinic.Common.Helpers;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;

namespace ChawlaClinic.BL.Services
{
    public class PaymentServiceRepo : IPaymentServiceRepo
    {
        private ApplicationDbContext _context;
        public PaymentServiceRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddPayment(CreatePaymentRequest dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string UserId = "";
                    var addUserId = _context.Users.Where(u => u.Id.ToString() == UserId).FirstOrDefault()?.Id;

                    if (addUserId == null) { throw new Exception(string.Format(CustomMessage.NOT_FOUND, "User")); }

                    var patient = _context.Patients.Where(p => p.Id.ToString() == dto.PatientId).FirstOrDefault();

                    if (patient == null) { return false; }

                    var payment = new Payment
                    {
                        PatientId = Guid.Parse(dto.PatientId),
                        AmountPaid = dto.AmountPaid,
                        PaymentDate = dto.PaymentDate,
                        IsActive = true,
                        IsDeleted = false,
                        AddedOn = DateTime.UtcNow,
                        AddedBy = addUserId ?? -1,
                        ModifiedOn = null,
                        ModifiedBy = null
                    };

                    payment.SecureToken = CommonHelper.GenerateSecureToken(payment.TokenID);

                    _context.Payments.Add(payment);

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

        public List<GetPaymentByPaymentIdRequest>? GetPayments(string patientId)
        {
            var patient = _context.Patients
                .Where(p => p.Id.ToString() == patientId &&
                            p.IsDeleted == false)
                .FirstOrDefault();

            if (patient == null) { return null; }

            var payments = _context.Payments
                .Where(p => p.PatientId == patient.Id &&
                            p.IsDeleted == false)
                .ToList();

            var paymentDtos = new List<GetPaymentByPaymentIdRequest>();

            foreach(var payment in payments)
            {
                paymentDtos.Add(new GetPaymentByPaymentIdRequest
                {
                    PaymentId = payment.Id.ToString(),
                    PatientId = payment.PatientId.ToString(),
                    AmountPaid = payment.AmountPaid,
                    PaymentDate = payment.PaymentDate,
                });
            }

            return paymentDtos;
        }

        public GetPaymentByPaymentIdRequest? GetPaymentById(string paymentId)
        {
            var payment = _context.Payments
                .Where(p => p.Id.ToString() == paymentId &&
                            p.IsDeleted == false)
                .Select(p => new GetPaymentByPaymentIdRequest
                {
                    PaymentId = p.Id.ToString(),
                    PatientId = p.PatientId.ToString(),
                    AmountPaid = p.AmountPaid,
                    PaymentDate = p.PaymentDate,
                })
                .FirstOrDefault();
            
            return payment;
        }
    }
}