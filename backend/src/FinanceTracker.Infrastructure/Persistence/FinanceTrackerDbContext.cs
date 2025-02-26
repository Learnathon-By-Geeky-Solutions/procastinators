using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

internal class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) 
    : IdentityDbContext<User>(options)
{
    internal DbSet<Wallet> Wallets { get; set; } = default!;
    internal DbSet<Category> Categories { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)");
        });
    }
}
