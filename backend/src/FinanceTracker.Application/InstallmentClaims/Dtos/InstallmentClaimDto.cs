using FinanceTracker.Application.Installments.Dtos;

namespace FinanceTracker.Application.InstallmentClaims.Dtos;

public class InstallmentClaimDto
{
    public int Id { get; set; } = default!;

    public InstallmentDto Installment { get; set; } = default!;
    public bool IsClaimed { get; set; } = default!;
    public DateTime? ClaimedAt { get; set; }
}
