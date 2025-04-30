using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories;

public class LoanRequestRepositoryTests : IDisposable
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly LoanRequestRepository _repository;
    private readonly string _testLenderId = "test-lender-id";
    private readonly string _testBorrowerId = "test-borrower-id";

    public LoanRequestRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new FinanceTrackerDbContext(options);
        _repository = new LoanRequestRepository(_dbContext);

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
    public async Task CreateAsync_ShouldAddLoanRequestToDatabase()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        // Act
        var createdId = await _repository.CreateAsync(request);

        // Assert
        Assert.True(createdId > 0);
        var savedRequest = await _dbContext.LoanRequests.FindAsync(createdId);
        Assert.NotNull(savedRequest);
        Assert.Equal(_testLenderId, savedRequest.LenderId);
        Assert.Equal(_testBorrowerId, savedRequest.BorrowerId);
        Assert.Equal(1000, savedRequest.Amount);
    }

    [Fact]
    public async Task GetAllReceivedAsync_ShouldReturnOnlyRequestsForLender()
    {
        // Arrange
        var requests = new[]
        {
            new LoanRequest
            {
                Id = 1,
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
            new LoanRequest
            {
                Id = 2,
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
            new LoanRequest
            {
                LenderId = "different-lender-id",
                Id = 3,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
        };

        await _dbContext.LoanRequests.AddRangeAsync(requests);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllReceivedAsync(_testLenderId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, r => Assert.Equal(_testLenderId, r.LenderId));
        Assert.DoesNotContain(result, r => r.Amount == 3000); // Different lender
    }

    [Fact]
    public async Task GetAllSentAsync_ShouldReturnOnlyRequestsFromBorrower()
    {
        // Arrange
        var requests = new[]
        {
            new LoanRequest
            {
                Id = 1,
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
            new LoanRequest
            {
                Id = 2,
                LenderId = _testLenderId,
                BorrowerId = _testBorrowerId,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
            new LoanRequest
            {
                LenderId = _testLenderId,
                BorrowerId = "different-borrower-id",
                Id = 3,
                Amount = 1000,
                RequestedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(4),
            },
        };

        await _dbContext.LoanRequests.AddRangeAsync(requests);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllSentAsync(_testBorrowerId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, r => Assert.Equal(_testBorrowerId, r.BorrowerId));
        Assert.DoesNotContain(result, r => r.Amount == 3000); // Different borrower
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRequest_WhenUserIsBorrower()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetByIdAsync(requestId, _testBorrowerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestId, result.Id);
        Assert.Equal(_testBorrowerId, result.BorrowerId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRequest_WhenUserIsLender()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetByIdAsync(requestId, _testLenderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestId, result.Id);
        Assert.Equal(_testLenderId, result.LenderId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserIsNeitherBorrowerNorLender()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetByIdAsync(requestId, "neither-borrower-nor-lender");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetReceivedByIdAsync_ShouldReturnRequest_WhenUserIsLender()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetReceivedByIdAsync(requestId, _testLenderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestId, result.Id);
        Assert.Equal(_testLenderId, result.LenderId);
    }

    [Fact]
    public async Task GetReceivedByIdAsync_ShouldReturnNull_WhenUserIsNotLender()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetReceivedByIdAsync(requestId, "different-lender-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSentByIdAsync_ShouldReturnRequest_WhenUserIsBorrower()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetSentByIdAsync(requestId, _testBorrowerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestId, result.Id);
        Assert.Equal(_testBorrowerId, result.BorrowerId);
    }

    [Fact]
    public async Task GetSentByIdAsync_ShouldReturnNull_WhenUserIsNotBorrower()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();
        var requestId = request.Id;

        // Act
        var result = await _repository.GetSentByIdAsync(requestId, "different-borrower-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        var request = new LoanRequest
        {
            Id = 1,
            LenderId = _testLenderId,
            BorrowerId = _testBorrowerId,
            Amount = 1000,
            RequestedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(4),
        };

        _dbContext.LoanRequests.Add(request);
        await _dbContext.SaveChangesAsync();

        // Modify the entity
        request.Amount = 1500;

        // Act
        var result = await _repository.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result); // One entity modified
        var updatedRequest = await _dbContext.LoanRequests.FindAsync(request.Id);
        Assert.NotNull(updatedRequest);
        Assert.Equal(1500, updatedRequest.Amount);
    }

    public void Dispose()
    {
        // Clean up resources after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
