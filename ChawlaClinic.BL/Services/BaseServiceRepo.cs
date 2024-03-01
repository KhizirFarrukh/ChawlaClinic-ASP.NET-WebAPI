using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.Common.Enums;
using ChawlaClinic.Common.Exceptions;
using ChawlaClinic.Common.Requests.Commons;
using ChawlaClinic.Common.Requests.Patient;
using ChawlaClinic.Common.Responses.Discounts;
using ChawlaClinic.Common.Responses.Patients;
using ChawlaClinic.DAL;
using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data.Entity;
using System.Linq.Dynamic.Core;

namespace ChawlaClinic.BL.Services
{
    public class BaseServiceRepo<T> : IBaseServiceRepo<T>
    {
        protected ApplicationDbContext _dbContext;

        public BaseServiceRepo(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> GetSequence()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var sequenceTableName = _dbContext.Model.FindEntityType(typeof(T))?.GetTableName();
                var sequence = _dbContext.Sequences.First(x => x.Name == sequenceTableName);
                var sequenceValue = (int)sequence.NextValue;
                sequence.NextValue += 1;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return sequenceValue;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
