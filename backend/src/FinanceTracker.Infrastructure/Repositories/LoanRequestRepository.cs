
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class LoanRequestRepository(FinanceTrackerDbContext dbContext) : ILoanRequestRepository
{
    public async Task<int> CreateAsync(LoanRequest request)
    {
        dbContext.LoanRequests.Add(request);
        await dbContext.SaveChangesAsync();
        return request.Id;
    }

    public async Task<IEnumerable<LoanRequest>> GetAllReceivedAsync(string userId)
    {
        return await dbContext.LoanRequests
            .Include(r => r.Borrower)
            .Where(r => r.LenderId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanRequest>> GetAllSentAsync(string userId)
    {
        return await dbContext.LoanRequests
            .Include(r => r.Lender)
            .Where(r => r.BorrowerId == userId)
            .ToListAsync();
    }

    public async Task<LoanRequest?> GetByIdAsync(int id)
    {
        return await dbContext.LoanRequests
            .Include(r => r.Borrower)
            .Include(r => r.Lender)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
