namespace FinanceTracker.Domain.Entities;

public class Loan
{
    public int Id { get; set; } = default!;
    public bool IsDeleted { get; set; } = default!;
    public string? LenderId { get; set; }
    public User? Lender { get; set; }

    public string? BorrowerId { get; set; }
    public User? Borrower { get; set; }

    public int? LoanRequestId { get; set; } = default!;
    public LoanRequest? LoanRequest { get; set; } = default!;

    public decimal Amount { get; set; } = default!;

    public string? Note { get; set; } = default!;

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; } = default!;
    public decimal DueAmount { get; set; } = default!;

    public int? LenderWalletId { get; set; } = default!;
    public Wallet LenderWallet { get; set; } = default!;

    public int? BorrowerWalletId { get; set; } = default!;
    public Wallet BorrowerWallet { get; set; } = default!;
}
