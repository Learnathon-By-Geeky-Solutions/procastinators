using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IWalletRepository
{
    Task<int> Create(Wallet wallet);
}
