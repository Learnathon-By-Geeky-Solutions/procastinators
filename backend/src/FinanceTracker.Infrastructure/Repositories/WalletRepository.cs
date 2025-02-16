using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;

namespace FinanceTracker.Infrastructure.Repositories;

internal class WalletRepository(FinanceTrackerDbContext dbContext) : IWalletRepository
{
    public Task<int> Create(Wallet wallet)
    {
        dbContext.Wallets.Add(wallet);
        return dbContext.SaveChangesAsync();
    }
}
