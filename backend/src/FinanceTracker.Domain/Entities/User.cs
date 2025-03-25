using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Domain.Entities;

public class User : IdentityUser
{
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public ICollection<Wallet>? Wallets { get; set; }
    public ICollection<Category>? Categories { get; set; }

    public ICollection<PersonalTransaction>? Transactions { get; set; }
}
