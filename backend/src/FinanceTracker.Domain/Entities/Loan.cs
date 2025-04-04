
namespace FinanceTracker.Domain.Entities;

public class Loan
{
    public int Id { get; set; } = default!;
    public bool IsDeleted { get; set; } = default!;
    public string LenderId { get; set; } = default!;
    public User Lender { get; set; } = default!;

    public int? LoanRequestId { get; set; } = default!;
    public LoanRequest? LoanRequest { get; set; } = default!;
    public decimal Amount { get; set; } = default!;

    public string? Note { get; set; } = default!;

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;


    public DateTime DueDate { get; set; } = default!;
    public decimal DueAmount { get; set; } = default!;

}
