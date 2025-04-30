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

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenInstallmentExistsButForDifferentLoan()
    {
        // Arrange
        var installment = new Installment
        {
            LoanId = 78, // Different loan ID
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
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllInstallmentClaimsAsync_ShouldReturnOnlyUserClaims()
    {
        // Clear out any existing data to ensure clean test
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();

        // Create loans with different lenders
        var userLoan = new Loan
        {
            Id = 100,
            Amount = 2000,
            LenderId = _testUserId, // Use test user ID (not "other-lender-id")
            BorrowerId = _testBorrowerId,
            DueAmount = 2000,
        };

        var otherLoan = new Loan
        {
            Id = 200,
            Amount = 2000,
            LenderId = "other-lender-id",
            BorrowerId = _testBorrowerId,
            DueAmount = 2300,
        };

        _dbContext.Loans.Add(userLoan);
        _dbContext.Loans.Add(otherLoan);
        await _dbContext.SaveChangesAsync();

        // Create installments for each loan
        var userInstallment = new Installment
        {
            LoanId = userLoan.Id,
            Amount = 120,
            Timestamp = DateTime.Now.AddMonths(1),
            NextDueDate = DateTime.Now.AddMonths(2),
            Note = "Test installment for user",
        };

        var otherInstallment = new Installment
        {
            LoanId = otherLoan.Id,
            Amount = 120,
            Timestamp = DateTime.Now.AddMonths(1),
            NextDueDate = DateTime.Now.AddMonths(2),
            Note = "Test installment for other",
        };

        _dbContext.Installments.Add(userInstallment);
        _dbContext.Installments.Add(otherInstallment);
        await _dbContext.SaveChangesAsync();

        // Create claims for each installment
        _dbContext.InstallmentClaims.Add(
            new InstallmentClaim { InstallmentId = userInstallment.Id }
        );
        _dbContext.InstallmentClaims.Add(
            new InstallmentClaim { InstallmentId = otherInstallment.Id }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllInstallmentClaimsAsync(_testUserId);

        // Assert
        Assert.Single(result);
        Assert.All(result, claim => Assert.Equal(_testUserId, claim.Installment.Loan.LenderId));
    }

    [Fact]
    public async Task GetInstallmentClaimByIdAsync_ShouldReturnClaim_WhenExists()
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

        var claim = new InstallmentClaim { InstallmentId = installment.Id };
        _dbContext.InstallmentClaims.Add(claim);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetInstallmentClaimByIdAsync(claim.Id, _testUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(claim.Id, result.Id);
        Assert.Equal(installment.Id, result.InstallmentId);
        Assert.Equal(_testUserId, result.Installment.Loan.LenderId);
    }

    [Fact]
    public async Task GetInstallmentClaimByIdAsync_ShouldReturnNull_WhenClaimDoesNotExist()
    {
        // Act
        var result = await _repository.GetInstallmentClaimByIdAsync(999, _testUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetInstallmentClaimByIdAsync_ShouldReturnNull_WhenClaimExistsButForDifferentUser()
    {
        // Arrange
        // Clear out any existing data to ensure clean test
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();

        // Create another loan with different lender
        var otherLoan = new Loan
        {
            Id = 300,
            Amount = 2000,
            LenderId = "other-lender-id", // Different from _testUserId
            BorrowerId = _testBorrowerId,
            DueAmount = 2000,
        };
        _dbContext.Loans.Add(otherLoan);
        await _dbContext.SaveChangesAsync();

        var installment = new Installment
        {
            LoanId = otherLoan.Id, // Use the other loan ID, not _testLoanId
            Amount = 120,
            Timestamp = DateTime.Now.AddMonths(1),
            NextDueDate = DateTime.Now.AddMonths(2),
            Note = "Test installment for other user",
        };
        _dbContext.Installments.Add(installment);
        await _dbContext.SaveChangesAsync();

        var claim = new InstallmentClaim { InstallmentId = installment.Id };
        _dbContext.InstallmentClaims.Add(claim);
        await _dbContext.SaveChangesAsync();

        // Act - Try to get the claim as _testUserId who is not the lender
        var result = await _repository.GetInstallmentClaimByIdAsync(claim.Id, _testUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges()
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

        // Modify the entity
        installment.Amount = 150;

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result); // One entity modified
        var updatedInstallment = await _dbContext.Installments.FindAsync(installment.Id);
        Assert.NotNull(updatedInstallment);
        Assert.Equal(150, updatedInstallment.Amount);
    }

    public void Dispose()
    {
        // Clean up resources after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
