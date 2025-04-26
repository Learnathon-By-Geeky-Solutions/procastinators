
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IInstallmentRepository
{
    Task<int> CreateAsync(Installment installment);
    Task<Installment?> GetByIdAsync(int id);
    Task<IEnumerable<Installment>> GetAllByLoanIdAsync(int loanId);
    Task<int> SaveChangesAsync();
}
