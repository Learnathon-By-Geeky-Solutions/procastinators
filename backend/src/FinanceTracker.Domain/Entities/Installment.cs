
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FinanceTracker.Domain.Entities;

public class Installment
{
    public int Id { get; set; } = default!;

    public int LoanId { get; set; } = default!;
    public Loan Loan { get; set; } = default!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public decimal Amount { get; set; } = default!;

    public string? Note { get; set; } = default!;

    public DateTime NextDueDate { get; set; } = default!;
} 
