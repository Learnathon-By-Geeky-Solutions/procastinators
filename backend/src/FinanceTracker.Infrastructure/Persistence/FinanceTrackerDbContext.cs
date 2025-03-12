using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

internal class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) 
    : IdentityDbContext<User>(options)
{
    internal DbSet<Wallet> Wallets { get; set; } = default!;
    internal DbSet<Category> Categories { get; set; } = default!;
    internal DbSet<PersonalTransaction> Transactions { get; set; } = default!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PersonalTransaction>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
        });



        modelBuilder.Entity<PersonalTransaction>()
           .HasOne(p => p.Wallet)
           .WithMany(w => w.Transactions)
           .HasForeignKey(p => p.WalletId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PersonalTransaction>()
            .HasOne(p => p.Category)
            .WithMany(w => w.Transactions)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PersonalTransaction>()
            .HasOne(p => p.User)
            .WithMany(w => w.Transactions)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
