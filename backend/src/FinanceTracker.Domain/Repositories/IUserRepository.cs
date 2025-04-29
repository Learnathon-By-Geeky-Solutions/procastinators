using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}
