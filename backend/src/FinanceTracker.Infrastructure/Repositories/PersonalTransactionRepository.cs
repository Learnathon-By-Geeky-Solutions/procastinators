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
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        public async Task<PersonalTransaction?> GetById(int id)
        {
            return await dbContext.PersonalTransactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(IEnumerable<TotalPerCategory>, decimal)> GetReportOnCategories(
            string userId,
            int days,
            string? type
        )
        {
            var query = dbContext.PersonalTransactions.Where(t =>
                t.UserId == userId && !t.IsDeleted && t.Timestamp >= DateTime.UtcNow.AddDays(-days)
            );

            if (type != null)
                query = query.Where(t => t.TransactionType == type);

            var grandTotal = await query.SumAsync(t => t.Amount);
            var categories = query
                .GroupBy(t => t.Category)
                .Select(g => new TotalPerCategory(g.Key, g.Sum(t => t.Amount)));

            return (categories, grandTotal);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
