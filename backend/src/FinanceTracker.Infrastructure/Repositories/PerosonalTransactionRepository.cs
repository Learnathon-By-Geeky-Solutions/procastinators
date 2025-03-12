using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.Repositories
{
    internal class PerosonalTransactionRepository(FinanceTrackerDbContext dbContext) : IPersonalTransactionRepository
    {
        public Task<int> Create(PersonalTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PersonalTransaction>> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PersonalTransaction?> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
