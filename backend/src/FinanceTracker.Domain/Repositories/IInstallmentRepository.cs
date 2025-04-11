
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IInstallmentRepository
{
    Task<int> CreateAsync(Installment installment);
    Task<Installment?> GetByIdAsync(int id);
    Task<IEnumerable<Installment>> GetAllAsync();
    Task<int> SaveChangesAsync();
}
