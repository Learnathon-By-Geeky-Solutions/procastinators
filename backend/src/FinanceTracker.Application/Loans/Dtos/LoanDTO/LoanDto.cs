namespace FinanceTracker.Application.Loans.Dtos.LoanDTO;

public class LoanDto
{
    public int Id { get; set; } = default!;
    public string LenderId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; } = default!;
    public decimal DueAmount { get; set; } = default!;
}
