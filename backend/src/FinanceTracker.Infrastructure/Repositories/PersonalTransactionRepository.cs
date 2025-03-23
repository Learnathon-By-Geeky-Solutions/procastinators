using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;

namespace FinanceTracker.Infrastructure.Repositories
{
    internal class PersonalTransactionRepository(FinanceTrackerDbContext dbContext) : IPersonalTransactionRepository
    {
        public async Task<int> Create(PersonalTransaction transaction)
        {
            dbContext.PersonalTransactions.Add(transaction);
            await dbContext.SaveChangesAsync();
            return transaction.Id;
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PersonalTransaction>> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonalTransaction?> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
