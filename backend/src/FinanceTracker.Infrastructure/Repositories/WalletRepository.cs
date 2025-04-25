using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class WalletRepository(FinanceTrackerDbContext dbContext) : IWalletRepository
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

    public async Task<int> UpdateBalance(Wallet wallet)
    {
        var existingWallet = await dbContext.Wallets.FirstOrDefaultAsync(w =>
            w.Id == wallet.Id && !w.IsDeleted
        );
        if (existingWallet != null)
        {
            throw new Exception("Wallet Not Found");
        }

        existingWallet!.Balance = wallet.Balance;
        return await dbContext.SaveChangesAsync();
    }
}
