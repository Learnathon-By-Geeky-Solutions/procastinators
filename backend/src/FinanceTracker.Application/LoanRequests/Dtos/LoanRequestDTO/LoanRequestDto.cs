namespace FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;

public class LoanRequestDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime DueDate { get; set; }

    public string BorrowerId { get; set; } = default!;
    public string LenderId { get; set; } = default!;
    public bool IsApproved { get; set; }
}
