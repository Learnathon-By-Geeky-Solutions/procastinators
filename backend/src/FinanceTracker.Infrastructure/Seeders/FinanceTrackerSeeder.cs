using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Seeders;

internal class FinanceTrackerSeeder(FinanceTrackerDbContext dbContext) : IFinanceTrackerSeeder
{
    public async Task SeedAsync()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
