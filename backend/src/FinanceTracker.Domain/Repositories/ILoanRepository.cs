using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ILoanRepository
{
    Task<int> CreateAsync(Loan loan);
    Task<int> CreateWithClaimAsync(Loan loan);

    Task<IEnumerable<Loan>> GetAllAsLenderAsync(string LenderId);
    Task<IEnumerable<Loan>> GetAllAsBorrowerAsync(string BorrowerId);

    Task<Loan?> GetByIdAsync(int id, string userId);
    Task<Loan?> GetByIdAsLenderAsync(int id, string lenderId);
    Task<Loan?> GetByIdAsBorrowerAsync(int id, string borrowerId);

    Task<IEnumerable<LoanClaim>> GetAllLoanClaimsAsync(string userId);
    Task<LoanClaim?> GetLoanClaimByIdAsync(int id, string userId);

    Task<int> SaveChangesAsync();
}
