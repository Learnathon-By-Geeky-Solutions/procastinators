namespace FinanceTracker.Domain.Entities;

public class InstallmentClaim
{
    public int Id { get; set; } = default!;
    public int InstallmentId { get; set; } = default!;
    public Installment Installment { get; set; } = default!;
    public bool IsClaimed { get; set; } = default!;
    public DateTime? ClaimedAt { get; set; }
}
