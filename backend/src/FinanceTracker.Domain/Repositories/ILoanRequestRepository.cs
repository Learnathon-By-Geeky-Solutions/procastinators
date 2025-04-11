
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ILoanRequestRepository
{
    Task<int> CreateAsync(LoanRequest request);
    Task<LoanRequest?> GetByIdAsync(int id);
    Task<IEnumerable<LoanRequest>> GetAllAsync();
    Task<int> SaveChangesAsync();
}
