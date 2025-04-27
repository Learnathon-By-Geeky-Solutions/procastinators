namespace FinanceTracker.Domain.Entities;

public class LoanClaim
{
    public int Id { get; set; } = default!;
    public int LoanId { get; set; } = default!;
    public Loan Loan { get; set; } = default!;
    public bool IsClaimed { get; set; } = default!;
    public DateTime? ClaimedAt { get; set; }
}
