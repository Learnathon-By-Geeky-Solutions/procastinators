using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Domain.Entities;

public class User : IdentityUser
{
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
