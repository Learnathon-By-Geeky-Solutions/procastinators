using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Seeders;

internal class FinanceTrackerSeeder(FinanceTrackerDbContext dbContext) : IFinanceTrackerSeeder
{
    public async Task SeedAsync()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
