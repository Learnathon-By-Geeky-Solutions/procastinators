
namespace FinanceTracker.Application.LoanRequests.Dtos.Loan;

public class LoanDto
{
    public int Id { get; set; }
    public string LenderId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime DueDate { get; set; }
    public decimal DueAmount { get; set; }
}
