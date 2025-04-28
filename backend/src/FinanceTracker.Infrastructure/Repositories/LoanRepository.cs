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

    public async Task<int> CreateWithClaimAsync(Loan loan)
    {
        dbContext.Loans.Add(loan);
        dbContext.LoanClaims.Add(new LoanClaim { Loan = loan });
        await dbContext.SaveChangesAsync();
        return loan.Id;
    }

    public async Task<IEnumerable<Loan>> GetAllAsLenderAsync(string LenderId)
    {
        return await dbContext
            .Loans.Include(l => l.Lender)
            .Include(l => l.Borrower)
            .Where(l => !l.IsDeleted)
            .Where(l => l.LenderId == LenderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetAllAsBorrowerAsync(string BorrowerId)
    {
        return await dbContext
            .Loans.Include(l => l.Lender)
            .Include(l => l.Borrower)
            .Where(l => !l.IsDeleted)
            .Where(L => L.BorrowerId == BorrowerId)
            .ToListAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id, string userId)
    {
        return await dbContext
            .Loans.Include(l => l.LoanRequest)
            .Include(l => l.Lender)
            .Include(l => l.Borrower)
            .Where(l => !l.IsDeleted)
            .Where(l => l.BorrowerId == userId || l.LenderId == userId)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan?> GetByIdAsLenderAsync(int id, string lenderId)
    {
        return await dbContext
            .Loans.Include(l => l.LoanRequest)
            .Include(l => l.Lender)
            .Include(l => l.Borrower)
            .Where(l => !l.IsDeleted)
            .Where(l => l.LenderId == lenderId)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan?> GetByIdAsBorrowerAsync(int id, string borrowerId)
    {
        return await dbContext
            .Loans.Include(l => l.LoanRequest)
            .Include(l => l.Lender)
            .Include(l => l.Borrower)
            .Where(l => !l.IsDeleted)
            .Where(l => l.BorrowerId == borrowerId)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();

    public async Task<IEnumerable<LoanClaim>> GetAllLoanClaimsAsync(string userId)
    {
        return await dbContext
            .LoanClaims.Include(lc => lc.Loan)
            .Where(lc => lc.Loan.BorrowerId == userId)
            .ToListAsync();
    }

    public async Task<LoanClaim?> GetLoanClaimByIdAsync(int id, string userId)
    {
        return await dbContext
            .LoanClaims.Include(lc => lc.Loan)
            .Where(lc => lc.Loan.BorrowerId == userId)
            .FirstOrDefaultAsync(lc => lc.Id == id);
    }
}
