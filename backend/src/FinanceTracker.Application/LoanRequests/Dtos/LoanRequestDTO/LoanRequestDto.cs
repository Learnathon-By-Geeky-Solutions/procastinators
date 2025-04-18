namespace FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;

public class LoanRequestDto
{
    public int Id { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;

    public string BorrowerId { get; set; } = default!;
    public string LenderId { get; set; } = default!;
    public bool IsApproved { get; set; } = false;
}
