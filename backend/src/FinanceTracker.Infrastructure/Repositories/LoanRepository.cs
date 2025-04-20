using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class LoanRepository(FinanceTrackerDbContext dbContext) : ILoanRepository
{
    public async Task<int> CreateAsync(Loan loan)
    {
        dbContext.Loans.Add(loan);
        await dbContext.SaveChangesAsync();
        return loan.Id;
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await dbContext.Loans.Include(l => l.LoanRequest).FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetAllAsync(String LenderId)
    {
        return await dbContext.Loans.Include(l => l.LoanRequest)
            .Where(l=>l.LenderId == LenderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanRequest>> GetAllByBorrowerAsync(String BorrowerId)
    {
        return await dbContext.LoanRequests.Include(lr=>lr.Borrower)
            .Where(lr => lr.BorrowerId == BorrowerId)
            .ToListAsync();
    }

    public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
