
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ILoanRepository
{
    Task<int> CreateAsync(Loan loan);
    Task<Loan?> GetByIdAsync(int id);
    Task<IEnumerable<Loan>> GetAllAsync(String LenderId);
    Task<int> SaveChangesAsync();
}
