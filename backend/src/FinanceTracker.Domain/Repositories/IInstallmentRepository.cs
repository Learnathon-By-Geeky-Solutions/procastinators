using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IInstallmentRepository
{
    Task<int> CreateAsync(Installment installment);
    Task<int> CreateWithClaimAsync(Installment installment);
    Task<Installment?> GetByIdAsync(int id);
    Task<IEnumerable<Installment>> GetAllAsync(int loanId);

    Task<IEnumerable<InstallmentClaim>> GetAllInstallmentClaimsAsync(string userId);
    Task<InstallmentClaim?> GetInstallmentClaimByIdAsync(int id, string userId);
    Task<int> SaveChangesAsync();
}
