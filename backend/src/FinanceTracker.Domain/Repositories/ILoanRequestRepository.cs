using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ILoanRequestRepository
{
    Task<int> CreateAsync(LoanRequest request);
    Task<LoanRequest?> GetByIdAsync(int id, string userId);
    Task<LoanRequest?> GetReceivedByIdAsync(int id, string userId);
    Task<LoanRequest?> GetSentByIdAsync(int id, string userId);
    Task<IEnumerable<LoanRequest>> GetAllReceivedAsync(string userId);
    Task<IEnumerable<LoanRequest>> GetAllSentAsync(string userId);
    Task<int> SaveChangesAsync();
}
