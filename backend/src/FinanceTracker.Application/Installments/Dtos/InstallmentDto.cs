using FinanceTracker.Application.Loans.Dtos.LoanDTO;

namespace FinanceTracker.Application.Installments.Dtos;

public class InstallmentDto
{
    public int Id { get; set; } = default!;
    public LoanDto Loan { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime NextDueDate { get; set; } = default!;
}
