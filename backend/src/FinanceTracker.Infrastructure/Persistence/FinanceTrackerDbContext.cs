using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static FinanceTracker.Infrastructure.Extensions.TypeConverters;

namespace FinanceTracker.Infrastructure.Persistence;

internal class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
    : IdentityDbContext<User>(options)
{
    private const string DecimalColumnType = "decimal(18, 2)";
    internal DbSet<Wallet> Wallets { get; set; } = default!;
    internal DbSet<Category> Categories { get; set; } = default!;
    internal DbSet<PersonalTransaction> PersonalTransactions { get; set; } = default!;
    internal DbSet<LoanRequest> LoanRequests { get; set; } = default!;
    internal DbSet<Loan> Loans { get; set; } = default!;
    internal DbSet<Installment> Installments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.Balance).HasColumnType(DecimalColumnType);
        });

        builder.Entity<PersonalTransaction>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType);
        });

        builder.Entity<LoanRequest>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType);
        });

        builder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType);
            entity.Property(e => e.DueAmount).HasColumnType(DecimalColumnType);
        });

        builder.Entity<Installment>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType);
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

        builder
            .Entity<LoanRequest>()
            .HasOne(lr => lr.Borrower)
            .WithMany()
            .HasForeignKey(lr => lr.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<LoanRequest>()
            .HasOne(lr => lr.Lender)
            .WithMany()
            .HasForeignKey(lr => lr.LenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Loan>()
            .HasOne(lr => lr.Lender)
            .WithMany()
            .HasForeignKey(lr => lr.LenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Loan>()
            .HasOne(lr => lr.LoanRequest)
            .WithOne()
            .HasForeignKey<Loan>(lr => lr.LoanRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Installment>()
            .HasOne(lr => lr.Loan)
            .WithMany()
            .HasForeignKey(lr => lr.LoanId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Entity<Loan>()
            .HasOne(l => l.Wallet)
            .WithMany()
            .HasForeignKey(l => l.WalletId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Entity<Loan>()
            .HasOne(l => l.BorrowerWallet)
            .WithMany()
            .HasForeignKey(l => l.BorrowerWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<LoanRequest>()
            .HasOne(lr => lr.Wallet)
            .WithMany()
            .HasForeignKey(lr => lr.WalletId)
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
