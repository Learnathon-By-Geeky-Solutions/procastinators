using FinanceTracker.Application.Users.Dtos;

namespace FinanceTracker.Application.Loans.Dtos.LoanDTO;

public class LoanDto
{
    public int Id { get; set; } = default!;
    public UserInfoDto? Lender { get; set; }
    public UserInfoDto? Borrower { get; set; }
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime IssuedAt { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;
    public decimal DueAmount { get; set; } = default!;
}
