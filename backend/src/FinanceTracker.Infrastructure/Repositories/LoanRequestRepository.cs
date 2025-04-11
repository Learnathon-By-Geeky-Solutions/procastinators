
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class LoanRequestRepository(FinanceTrackerDbContext dbContext) : ILoanRequestRepository
{
    public async Task<int> CreateAsync(LoanRequest loanRequest)
    {
        dbContext.LoanRequests.Add(loanRequest);
        await dbContext.SaveChangesAsync();
        return loanRequest.Id;
    }

    public async Task<IEnumerable<LoanRequest>> GetAllAsync()
    {
        return await dbContext.LoanRequests
            .Include(r => r.Borrower)
            .Include(r => r.Lender)
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
