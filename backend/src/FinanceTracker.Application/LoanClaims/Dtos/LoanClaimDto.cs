using FinanceTracker.Application.Loans.Dtos.LoanDTO;

namespace FinanceTracker.Application.LoanClaims.Dtos;

public class LoanClaimDto
{
    public int Id { get; set; } = default!;

    public LoanDto Loan { get; set; } = default!;
    public bool IsClaimed { get; set; } = default!;
    public DateTime? ClaimedAt { get; set; }
}
