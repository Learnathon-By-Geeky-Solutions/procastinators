using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class InstallmentRepository(FinanceTrackerDbContext dbContext) : IInstallmentRepository
{
    public async Task<int> CreateAsync(Installment installment)
    {
        dbContext.Installments.Add(installment);
        await dbContext.SaveChangesAsync();
        return installment.Id;
    }

    public async Task<int> CreateWithClaimAsync(Installment installment)
    {
        dbContext.Installments.Add(installment);
        dbContext.InstallmentClaims.Add(new InstallmentClaim { Installment = installment });
        await dbContext.SaveChangesAsync();
        return installment.Id;
    }

    public async Task<IEnumerable<Installment>> GetAllAsync(int loanId)
    {
        return await dbContext.Installments.Where(i => i.LoanId == loanId).ToListAsync();
    }

    public async Task<Installment?> GetByIdAsync(int loanId, int id)
    {
        return await dbContext
            .Installments.Where(i => i.LoanId == loanId)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InstallmentClaim>> GetAllInstallmentClaimsAsync(string userId)
    {
        return await dbContext
            .InstallmentClaims.Include(ic => ic.Installment)
            .ThenInclude(i => i.Loan)
            .Where(ic => ic.Installment.Loan.LenderId == userId)
            .ToListAsync();
    }

    public async Task<InstallmentClaim?> GetInstallmentClaimByIdAsync(int id, string userId)
    {
        return await dbContext
            .InstallmentClaims.Include(ic => ic.Installment)
            .ThenInclude(i => i.Loan)
            .Where(ic => ic.Installment.Loan.LenderId == userId)
            .FirstOrDefaultAsync(ic => ic.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
