using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class WalletRepository(FinanceTrackerDbContext dbContext) : IWalletRepository
{
    public async Task<int> Create(Wallet wallet)
    {
        dbContext.Wallets.Add(wallet);
        await dbContext.SaveChangesAsync();
        return wallet.Id;
    }

    public async Task<IEnumerable<Wallet>> GetAll(string userId)
    {
        return await dbContext.Wallets.Where(w => w.UserId == userId && !w.IsDeleted).ToListAsync();
    }

    public async Task<Wallet?> GetById(int id)
    {
        return await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}
