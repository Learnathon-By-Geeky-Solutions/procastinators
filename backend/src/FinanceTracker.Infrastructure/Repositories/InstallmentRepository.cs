
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class InstallmentRepository(FinanceTrackerDbContext dbContext) : IInstallmentRepository
{
    public async Task<int> CreateAsync(Installment installment)
    {
        dbContext.Installments.Add(installment);
        await dbContext.SaveChangesAsync();
        return installment.Id;
    }

    public async Task<IEnumerable<Installment>> GetAllByLoanIdAsync(int loanId)
    {
        return await dbContext.Installments
            .Where(i => i.LoanId == loanId)
            .ToListAsync();
    }

    public async Task<Installment?> GetByIdAsync(int id)
    {
        return await dbContext.Installments
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
