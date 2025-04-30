using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static FinanceTracker.Infrastructure.Extensions.TypeConverters;

namespace FinanceTracker.Infrastructure.Persistence;

public class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
    : IdentityDbContext<User>(options)
{
    private const string DecimalColumnType = "decimal(18, 2)";
    public DbSet<Wallet> Wallets { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<PersonalTransaction> PersonalTransactions { get; set; } = default!;
    public DbSet<Loan> Loans { get; set; } = default!;
    public DbSet<LoanRequest> LoanRequests { get; set; } = default!;
    public DbSet<LoanClaim> LoanClaims { get; set; } = default!;
    public DbSet<Installment> Installments { get; set; } = default!;
    public DbSet<InstallmentClaim> InstallmentClaims { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureDecimalColumns(builder);

        ConfigurePersonalTransactionRelations(builder);

        ConfigureLoanRelations(builder);

        ConfigureLoanRequestRelations(builder);

        builder
            .Entity<LoanClaim>()
            .HasOne(lc => lc.Loan)
            .WithOne()
            .HasForeignKey<LoanClaim>(lc => lc.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Installment>()
            .HasOne(lr => lr.Loan)
            .WithMany()
            .HasForeignKey(lr => lr.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<InstallmentClaim>()
            .HasOne(ic => ic.Installment)
            .WithOne()
            .HasForeignKey<InstallmentClaim>(ic => ic.InstallmentId)
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

    private static void ConfigureDecimalColumns(ModelBuilder builder)
    {
        builder.Entity<Wallet>(entity =>
            entity.Property(e => e.Balance).HasColumnType(DecimalColumnType)
        );

        builder.Entity<PersonalTransaction>(entity =>
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType)
        );

        builder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType);
            entity.Property(e => e.DueAmount).HasColumnType(DecimalColumnType);
        });

        builder.Entity<LoanRequest>(entity =>
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType)
        );

        builder.Entity<Installment>(entity =>
            entity.Property(e => e.Amount).HasColumnType(DecimalColumnType)
        );
    }

    private static void ConfigurePersonalTransactionRelations(ModelBuilder builder)
    {
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

    private static void ConfigureLoanRelations(ModelBuilder builder)
    {
        builder
            .Entity<Loan>()
            .HasOne(lr => lr.Lender)
            .WithMany()
            .HasForeignKey(lr => lr.LenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Loan>()
            .HasOne(lr => lr.Borrower)
            .WithMany()
            .HasForeignKey(lr => lr.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Loan>()
            .HasOne(lr => lr.LoanRequest)
            .WithOne()
            .HasForeignKey<Loan>(lr => lr.LoanRequestId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureLoanRequestRelations(ModelBuilder builder)
    {
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
    }
}
