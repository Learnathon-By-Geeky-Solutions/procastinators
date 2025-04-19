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
<<<<<<< HEAD
    internal DbSet<LoanRequest> LoanRequests { get; set; } = default!;
    internal DbSet<Loan> Loans { get; set; } = default!;
    internal DbSet<Installment> Installments { get; set; } = default!;
   


    protected override void OnModelCreating(ModelBuilder modelBuilder)
=======

    protected override void OnModelCreating(ModelBuilder builder)
>>>>>>> 641356cf7b782af66b88a573ad3bdb1852d6ac3b
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

<<<<<<< HEAD
        modelBuilder.Entity<LoanRequest>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
        });


        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.DueAmount)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Installment>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
        });

=======
        builder
            .Entity<PersonalTransaction>()
            .HasOne(p => p.Wallet)
            .WithMany()
            .HasForeignKey(p => p.WalletId)
            .OnDelete(DeleteBehavior.Restrict);
>>>>>>> 641356cf7b782af66b88a573ad3bdb1852d6ac3b

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



        modelBuilder.Entity<LoanRequest>()
            .HasOne(lr => lr.Borrower)
            .WithMany()
            .HasForeignKey(lr => lr.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LoanRequest>()
            .HasOne(lr => lr.Lender)
            .WithMany()
            .HasForeignKey(lr => lr.LenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
            .HasOne(lr => lr.Lender)
            .WithMany()
            .HasForeignKey(lr => lr.LenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
           .HasOne(lr => lr.LoanRequest)
           .WithOne()
           .HasForeignKey<Loan>(lr => lr.LoanRequestId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Installment>()
           .HasOne(lr => lr.Loan)
           .WithMany()
           .HasForeignKey(lr => lr.LoanId)
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
