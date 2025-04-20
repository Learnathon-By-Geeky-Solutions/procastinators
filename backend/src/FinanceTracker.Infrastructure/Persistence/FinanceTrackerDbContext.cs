using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static FinanceTracker.Infrastructure.Extensions.TypeConverters;

namespace FinanceTracker.Infrastructure.Persistence;

internal class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
    : IdentityDbContext<User>(options)
{
    internal DbSet<Wallet> Wallets { get; set; } = default!;
    internal DbSet<Category> Categories { get; set; } = default!;
    internal DbSet<PersonalTransaction> PersonalTransactions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
        });

        builder.Entity<PersonalTransaction>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
        });

        builder
            .Entity<PersonalTransaction>()
            .HasOne(p => p.Wallet)
            .WithMany()
            .HasForeignKey(p => p.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<PersonalTransaction>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<PersonalTransaction>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
        configurationBuilder
            .Properties<DateTime?>()
            .HaveConversion<NullableDateTimeAsUtcValueConverter>();
    }
}
