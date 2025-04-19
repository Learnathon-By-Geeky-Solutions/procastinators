using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Domain.Entities;

public class User : IdentityUser
{
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
<<<<<<< HEAD
    public ICollection<Wallet>? Wallets { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<PersonalTransaction>? Transactions { get; set; }
    public ICollection<LoanRequest>? LoanRequests { get; set; }
    public ICollection<Loan>? Loans { get; set; }
=======
>>>>>>> 641356cf7b782af66b88a573ad3bdb1852d6ac3b
}
