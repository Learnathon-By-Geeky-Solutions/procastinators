using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories
{
    internal class PersonalTransactionRepository(FinanceTrackerDbContext dbContext)
        : IPersonalTransactionRepository
    {
        public async Task<int> Create(PersonalTransaction transaction)
        {
            dbContext.PersonalTransactions.Add(transaction);
            await dbContext.SaveChangesAsync();
            return transaction.Id;
        }

        public async Task<IEnumerable<PersonalTransaction>> GetAll(string userId)
        {
            return await dbContext
                .PersonalTransactions.Where(t => t.UserId == userId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<PersonalTransaction?> GetById(int id)
        {
            return await dbContext.PersonalTransactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<int> SaveChangeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
