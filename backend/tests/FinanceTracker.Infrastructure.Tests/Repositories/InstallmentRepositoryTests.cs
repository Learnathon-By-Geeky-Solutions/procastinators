using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories;

public class InstallmentRepositoryTests : IDisposable
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly InstallmentRepository _repository;
    private readonly string _testUserId = "test-user-id";
    private readonly string _testBorrowerId = "test-borrower-id";
    private int _testLoanId;

    public InstallmentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new FinanceTrackerDbContext(options);
        _repository = new InstallmentRepository(_dbContext);

        // Set up a test loan for use in tests
        SetupTestLoan().GetAwaiter().GetResult();
    }

    private async Task SetupTestLoan()
    {
        var loan = new Loan
        {
            Amount = 1000,
            LenderId = _testUserId,
            BorrowerId = _testBorrowerId,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        _testLoanId = loan.Id;
    }

    public void Dispose()
    {
        ((IDisposable)_dbContext).Dispose();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddInstallmentToDatabase()
    {
        // Arrange
        var installment = new Installment { LoanId = _testLoanId, Amount = 100 };

        // Act
        var createdId = await _repository.CreateAsync(installment);

        // Assert
        Assert.True(createdId > 0);
        var savedInstallment = await _dbContext.Installments.FindAsync(createdId);
        Assert.NotNull(savedInstallment);
        Assert.Equal(_testLoanId, savedInstallment.LoanId);
        Assert.Equal(100, savedInstallment.Amount);
    }

    [Fact]
    public async Task CreateWithClaimAsync_ShouldAddInstallmentAndClaimToDatabase()
    {
        // Arrange
        var installment = new Installment
        {
            LoanId = _testLoanId,
            Amount = 150,
            Timestamp = DateTime.Now.AddMonths(2),
            NextDueDate = DateTime.Now.AddMonths(3),
            Note = "Test installment",
        };

        // Act
        var createdId = await _repository.CreateWithClaimAsync(installment);

        // Assert
        Assert.True(createdId > 0);

        // Check installment was created
        var savedInstallment = await _dbContext.Installments.FindAsync(createdId);
        Assert.NotNull(savedInstallment);
        Assert.Equal(_testLoanId, savedInstallment.LoanId);

        // Check claim was created
        var claim = await _dbContext.InstallmentClaims.FirstOrDefaultAsync(c =>
            c.Installment.Id == createdId
        );
        Assert.NotNull(claim);
        Assert.Equal(createdId, claim.Installment.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyLoanInstallments()
    {
        // Arrange
        var installments = new[]
        {
            new Installment
            {
                LoanId = _testLoanId,
                Amount = 150,
                Timestamp = DateTime.Now.AddMonths(2),
                NextDueDate = DateTime.Now.AddMonths(3),
                Note = "Test installment",
            },
            new Installment
            {
                LoanId = _testLoanId,
                Amount = 150,
                Timestamp = DateTime.Now.AddMonths(1),
                NextDueDate = DateTime.Now.AddMonths(2),
                Note = "Test installment212",
            },
            new Installment
            {
                LoanId = 99, // Different loan ID
                Amount = 150,
                Timestamp = DateTime.Now,
                NextDueDate = DateTime.Now.AddMonths(2),
                Note = "Test installment2121",
            },
        };

        await _dbContext.Installments.AddRangeAsync(installments);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(_testLoanId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, i => Assert.Equal(_testLoanId, i.LoanId));
        Assert.DoesNotContain(result, i => i.LoanId == 99);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnInstallment_WhenExists()
    {
        // Arrange
        var installment = new Installment
        {
            LoanId = _testLoanId,
            Amount = 120,
            Timestamp = DateTime.Now.AddMonths(1),
            NextDueDate = DateTime.Now.AddMonths(2),
            Note = "Test installment212",
        };

        _dbContext.Installments.Add(installment);
        await _dbContext.SaveChangesAsync();
        var installmentId = installment.Id;

        // Act
        var result = await _repository.GetByIdAsync(_testLoanId, installmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(installmentId, result.Id);
        Assert.Equal(_testLoanId, result.LoanId);
        Assert.Equal(120, result.Amount);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenInstallmentDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(_testLoanId, 999);

        // Assert
        Assert.Null(result);
    }
}
