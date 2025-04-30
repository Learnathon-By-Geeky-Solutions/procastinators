using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories;

public class LoanRepositoryTests : IDisposable
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly LoanRepository _repository;
    private readonly string _testLenderId = "test-lender-id";
    private readonly string _testBorrowerId = "test-borrower-id";

    public LoanRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new FinanceTrackerDbContext(options);
        _repository = new LoanRepository(_dbContext);

        // Set up test users for later use
        SetupTestUsers().GetAwaiter().GetResult();
    }

    private async Task SetupTestUsers()
    {
        var lender = new User { Id = _testLenderId, UserName = "testlender@example.com" };

        var borrower = new User { Id = _testBorrowerId, UserName = "testborrower@example.com" };

        _dbContext.Users.Add(lender);
        _dbContext.Users.Add(borrower);
        await _dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddLoanToDatabase()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            DueAmount = 1000,
        };

        // Act
        var createdId = await _repository.CreateAsync(loan);

        // Assert
        Assert.True(createdId > 0);
        var savedLoan = await _dbContext.Loans.FindAsync(createdId);
        Assert.NotNull(savedLoan);
        Assert.Equal(_testLenderId, savedLoan.LenderId);
        Assert.Equal(_testBorrowerId, savedLoan.BorrowerId);
        Assert.Equal(1000, savedLoan.Amount);
    }

    [Fact]
    public async Task CreateWithClaimAsync_ShouldAddLoanAndClaimToDatabase()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1500,
            DueAmount = 1500,
        };

        // Act
        var createdId = await _repository.CreateWithClaimAsync(loan);

        // Assert
        Assert.True(createdId > 0);

        // Check loan was created
        var savedLoan = await _dbContext.Loans.FindAsync(createdId);
        Assert.NotNull(savedLoan);
        Assert.Equal(_testLenderId, savedLoan.LenderId);
        Assert.Equal(_testBorrowerId, savedLoan.BorrowerId);

        // Check claim was created
        var claim = await _dbContext.LoanClaims.FirstOrDefaultAsync(c => c.LoanId == createdId);
        Assert.NotNull(claim);
        Assert.Equal(createdId, claim.LoanId);
    }

    [Fact]
    public async Task GetAllAsLenderAsync_ShouldReturnOnlyNonDeletedLoansForLender()
    {
        // Arrange
        var loans = new[]
        {
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                IsDeleted = false,
            },
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 2000,
                IsDeleted = false,
            },
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 3000,
                IsDeleted = true, // This one should be excluded
            },
            new Loan
            {
                LenderId = "different-lender-id",
                BorrowerId = _testBorrowerId,
                Amount = 4000,
                IsDeleted = false, // This one should be excluded (different lender)
            },
        };

        await _dbContext.Loans.AddRangeAsync(loans);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsLenderAsync(_testLenderId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, l => Assert.Equal(_testLenderId, l.LenderId));
        Assert.All(result, l => Assert.False(l.IsDeleted));
        Assert.DoesNotContain(result, l => l.Amount == 3000); // Deleted loan
        Assert.DoesNotContain(result, l => l.Amount == 4000); // Different lender
    }

    [Fact]
    public async Task GetAllAsBorrowerAsync_ShouldReturnOnlyNonDeletedLoansForBorrower()
    {
        // Arrange
        var loans = new[]
        {
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                IsDeleted = false,
            },
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 2000,
                IsDeleted = false,
            },
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 3000,
                IsDeleted = true, // This one should be excluded
            },
            new Loan
            {
                LenderId = _testLenderId,
                BorrowerId = "different-borrower-id",
                Amount = 4000,
                IsDeleted = false, // This one should be excluded (different borrower)
            },
        };

        await _dbContext.Loans.AddRangeAsync(loans);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsBorrowerAsync(_testBorrowerId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, l => Assert.Equal(_testBorrowerId, l.BorrowerId));
        Assert.All(result, l => Assert.False(l.IsDeleted));
        Assert.DoesNotContain(result, l => l.Amount == 3000); // Deleted loan
        Assert.DoesNotContain(result, l => l.Amount == 4000); // Different borrower
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLoan_WhenUserIsBorrower()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsync(loanId, _testBorrowerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loanId, result.Id);
        Assert.Equal(_testBorrowerId, result.BorrowerId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLoan_WhenUserIsLender()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsync(loanId, _testLenderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loanId, result.Id);
        Assert.Equal(_testLenderId, result.LenderId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserIsNeitherBorrowerNorLender()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsync(loanId, "neither-borrower-nor-lender");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenLoanIsDeleted()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = true,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsync(loanId, _testBorrowerId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsLenderAsync_ShouldReturnLoan_WhenUserIsLender()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsLenderAsync(loanId, _testLenderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loanId, result.Id);
        Assert.Equal(_testLenderId, result.LenderId);
    }

    [Fact]
    public async Task GetByIdAsLenderAsync_ShouldReturnNull_WhenUserIsNotLender()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsLenderAsync(loanId, "different-lender-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsLenderAsync_ShouldReturnNull_WhenLoanIsDeleted()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = true,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsLenderAsync(loanId, _testLenderId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsBorrowerAsync_ShouldReturnLoan_WhenUserIsBorrower()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsBorrowerAsync(loanId, _testBorrowerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loanId, result.Id);
        Assert.Equal(_testBorrowerId, result.BorrowerId);
    }

    [Fact]
    public async Task GetByIdAsBorrowerAsync_ShouldReturnNull_WhenUserIsNotBorrower()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsBorrowerAsync(loanId, "different-borrower-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsBorrowerAsync_ShouldReturnNull_WhenLoanIsDeleted()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = true,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        var loanId = loan.Id;

        // Act
        var result = await _repository.GetByIdAsBorrowerAsync(loanId, _testBorrowerId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            DueAmount = 1000,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        // Modify the entity
        loan.Amount = 1500;
        loan.DueAmount = 1500;

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result); // One entity modified
        var updatedLoan = await _dbContext.Loans.FindAsync(loan.Id);
        Assert.NotNull(updatedLoan);
        Assert.Equal(1500, updatedLoan.Amount);
        Assert.Equal(1500, updatedLoan.DueAmount);
    }

    [Fact]
    public async Task GetAllLoanClaimsAsync_ShouldReturnOnlyUserClaims()
    {
        // Arrange
        // Create a fresh context for this test to avoid tracking issues
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(
                databaseName: "GetAllLoanClaimsAsync_Test_" + Guid.NewGuid().ToString()
            )
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new FinanceTrackerDbContext(options);
        var repository = new LoanRepository(context);

        // Create test users
        var lender = new User { Id = _testLenderId, UserName = "testlender@example.com" };
        var borrower = new User { Id = _testBorrowerId, UserName = "testborrower@example.com" };
        var differentLender = new User
        {
            Id = "different-lender",
            UserName = "different@example.com",
        };

        context.Users.Add(lender);
        context.Users.Add(borrower);
        context.Users.Add(differentLender);
        await context.SaveChangesAsync();

        // Create loans for different borrowers
        var loan1 = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        var loan2 = new Loan
        {
            LenderId = differentLender.Id,
            BorrowerId = _testBorrowerId,
            Amount = 2000,
            IsDeleted = false,
        };

        context.Loans.Add(loan1);
        context.Loans.Add(loan2);
        await context.SaveChangesAsync();

        // Create loan claims
        context.LoanClaims.Add(new LoanClaim { LoanId = loan1.Id });
        context.LoanClaims.Add(new LoanClaim { LoanId = loan2.Id });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllLoanClaimsAsync(_testBorrowerId);

        // Assert
        Assert.Equal(2, result.Count()); // Should return claims for both loans where user is borrower
        Assert.All(result, claim => Assert.Equal(_testBorrowerId, claim.Loan.BorrowerId));
    }

    [Fact]
    public async Task GetLoanClaimByIdAsync_ShouldReturnClaim_WhenExists()
    {
        // Arrange
        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            IsDeleted = false,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        var claim = new LoanClaim { LoanId = loan.Id };
        _dbContext.LoanClaims.Add(claim);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetLoanClaimByIdAsync(claim.Id, _testBorrowerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(claim.Id, result.Id);
        Assert.Equal(loan.Id, result.LoanId);
        Assert.Equal(_testBorrowerId, result.Loan.BorrowerId);
    }

    [Fact]
    public async Task GetLoanClaimByIdAsync_ShouldReturnNull_WhenClaimDoesNotExist()
    {
        // Act
        var result = await _repository.GetLoanClaimByIdAsync(999, _testBorrowerId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetLoanClaimByIdAsync_ShouldReturnNull_WhenClaimExistsButForDifferentUser()
    {
        // Arrange
        // Create a fresh context for this test to avoid tracking issues
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(
                databaseName: "GetLoanClaimByIdAsync_Test_" + Guid.NewGuid().ToString()
            )
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new FinanceTrackerDbContext(options);
        var repository = new LoanRepository(context);

        // Create test users
        var lender = new User { Id = _testLenderId, UserName = "testlender@example.com" };
        var borrower = new User { Id = _testBorrowerId, UserName = "testborrower@example.com" };
        var differentBorrower = new User
        {
            Id = "different-borrower-id",
            UserName = "different@example.com",
        };

        context.Users.Add(lender);
        context.Users.Add(borrower);
        context.Users.Add(differentBorrower);
        await context.SaveChangesAsync();

        var loan = new Loan
        {
            LenderId = _testLenderId,
            BorrowerId = differentBorrower.Id, // Different from _testBorrowerId
            Amount = 1000,
            IsDeleted = false,
        };

        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var claim = new LoanClaim { LoanId = loan.Id };
        context.LoanClaims.Add(claim);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetLoanClaimByIdAsync(claim.Id, _testBorrowerId);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        // Clean up resources after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
