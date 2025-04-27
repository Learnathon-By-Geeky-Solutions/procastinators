using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ILoanRepository
{
    Task<int> CreateAsync(Loan loan);
    Task<int> CreateWithClaimAsync(Loan loan);
    Task<Loan?> GetByIdAsync(int id);
    Task<IEnumerable<Loan>> GetAllAsLenderAsync(String LenderId);

    Task<IEnumerable<Loan>> GetAllAsBorrowerAsync(String BorrowerId);
    Task<IEnumerable<LoanClaim>> GetAllLoanClaimsAsync(string userId);
    Task<LoanClaim?> GetLoanClaimByIdAsync(int id, string userId);

    Task<int> SaveChangesAsync();
}
