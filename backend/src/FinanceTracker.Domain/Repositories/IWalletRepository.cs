using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IWalletRepository
{
    Task<IEnumerable<Wallet>> GetAll(string userId);
    Task<Wallet?> GetById(int id);
    Task<int> Create(Wallet wallet);
    Task<int> SaveChangesAsync();
    Task<int> UpdateBalance(Wallet wallet);
}
