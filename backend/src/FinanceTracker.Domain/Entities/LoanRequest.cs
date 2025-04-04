using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Domain.Entities
{
    public class LoanRequest
    {
        public int Id { get; set; } = default!;

        public decimal Amount { get; set; } = default!;

        public string? Note { get; set; } = default!;

        public DateTime DueDate { get; set; } = default!;

        public string BorrowerId { get; set; } = default!;
        public User Borrower { get; set; } = default!;

        public string LenderId { get; set; } = default!;
        public User Lender { get; set; } = default!;

        public bool IsApproved { get; set; } = false;
    }
}
