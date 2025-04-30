using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories;

public class PersonalTransactionRepositoryTests : IDisposable
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly PersonalTransactionRepository _repository;
    private readonly string _testUserId = "test-user-id";
    private readonly int _testWalletId = 1;
    private readonly int _testCategoryId = 1;

    public PersonalTransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new FinanceTrackerDbContext(options);
        _repository = new PersonalTransactionRepository(_dbContext);

        // Set up test data for later use
        SetupTestData().GetAwaiter().GetResult();
    }

    private async Task SetupTestData()
    {
        var user = new User { Id = _testUserId, UserName = "testuser@example.com" };
        var wallet = new Wallet
        {
            Id = _testWalletId,
            Name = "Test Wallet",
            UserId = _testUserId,
            User = user,
            Currency = "USD", // Add the missing required property
            Type = "Cash", // Add the missing required property
        };
        var category = new Category
        {
            Id = _testCategoryId,
            Title = "Test Category",
            UserId = _testUserId,
            User = user,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };

        _dbContext.Users.Add(user);
        _dbContext.Wallets.Add(wallet);
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Create_ShouldAddTransactionToDatabase()
    {
        // Arrange
        var transaction = new PersonalTransaction
        {
            UserId = _testUserId,
            Amount = 100.50m,
            CategoryId = _testCategoryId,
            WalletId = _testWalletId,
            Note = "Weekly shopping",
            TransactionType = "Expense",
            Timestamp = DateTime.UtcNow,
            IsDeleted = false,
        };

        // Act
        var createdId = await _repository.Create(transaction);

        // Assert
        Assert.True(createdId > 0);
        var savedTransaction = await _dbContext.PersonalTransactions.FindAsync(createdId);
        Assert.NotNull(savedTransaction);
        Assert.Equal(_testUserId, savedTransaction.UserId);
        Assert.Equal(100.50m, savedTransaction.Amount);
        Assert.Equal(_testCategoryId, savedTransaction.CategoryId);
        Assert.Equal(_testWalletId, savedTransaction.WalletId);
        Assert.Equal("Expense", savedTransaction.TransactionType);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOnlyUserTransactionsThatAreNotDeleted()
    {
        // Arrange
        var transactions = new[]
        {
            new PersonalTransaction
            {
                Id = 101, // Explicitly set ID
                UserId = _testUserId,
                Amount = 100.50m,
                CategoryId = _testCategoryId,
                WalletId = _testWalletId,
                Note = "Weekly shopping",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 102, // Explicitly set ID
                UserId = _testUserId,
                Amount = 50.25m,
                CategoryId = _testCategoryId,
                WalletId = _testWalletId,
                Note = "Movie tickets",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 103, // Explicitly set ID
                UserId = _testUserId,
                Amount = 75.00m,
                CategoryId = _testCategoryId,
                WalletId = _testWalletId,
                Note = "Birthday gift",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow,
                IsDeleted = true, // Deleted transaction
            },
            new PersonalTransaction
            {
                Id = 104, // Explicitly set ID
                UserId = "different-user-id",
                Amount = 200.00m,
                CategoryId = _testCategoryId,
                WalletId = _testWalletId,
                Note = "Monthly salary",
                TransactionType = "Income",
                Timestamp = DateTime.UtcNow,
                IsDeleted = false,
            },
        };

        await _dbContext.PersonalTransactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll(_testUserId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.Equal(_testUserId, t.UserId));
        Assert.All(result, t => Assert.False(t.IsDeleted));
        Assert.DoesNotContain(result, t => t.Amount == 200.00m); // Different user
        Assert.DoesNotContain(result, t => t.Amount == 75.00m); // Deleted transaction

        // Verify descending order by timestamp
        var orderedResult = result.ToList();
        Assert.Equal(50.25m, orderedResult[0].Amount); // Most recent non-deleted first
        Assert.Equal(100.50m, orderedResult[1].Amount); // Oldest last
    }

    [Fact]
    public async Task GetById_ShouldReturnTransaction_WhenTransactionExists()
    {
        // Arrange
        var transaction = new PersonalTransaction
        {
            Id = 201, // Explicitly set ID
            UserId = _testUserId,
            Amount = 100.50m,
            CategoryId = _testCategoryId,
            WalletId = _testWalletId,
            Note = "Weekly shopping",
            TransactionType = "Expense",
            Timestamp = DateTime.UtcNow,
            IsDeleted = false,
        };

        _dbContext.PersonalTransactions.Add(transaction);
        await _dbContext.SaveChangesAsync();
        var transactionId = transaction.Id;

        // Act
        var result = await _repository.GetById(transactionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(transactionId, result.Id);
        Assert.Equal(_testUserId, result.UserId);
        Assert.Equal(100.50m, result.Amount);
        Assert.Equal(_testCategoryId, result.CategoryId);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenTransactionDoesNotExist()
    {
        // Act
        var result = await _repository.GetById(999); // Non-existent ID

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetReportOnCategories_ShouldReturnCorrectTotals_ForAllTypes()
    {
        // Arrange
        var groceryCategory = new Category
        {
            Id = 2,
            Title = "Groceries",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var entertainmentCategory = new Category
        {
            Id = 3,
            Title = "Entertainment",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var salaryCategory = new Category
        {
            Id = 4,
            Title = "Salary",
            UserId = _testUserId,
            DefaultTransactionType = "Income",
            IsDeleted = false,
        };

        _dbContext.Categories.AddRange(groceryCategory, entertainmentCategory, salaryCategory);
        await _dbContext.SaveChangesAsync();

        var transactions = new[]
        {
            new PersonalTransaction
            {
                Id = 301, // Explicitly set ID
                UserId = _testUserId,
                Amount = 100.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Weekly shopping",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 302, // Explicitly set ID
                UserId = _testUserId,
                Amount = 50.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Extra items",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 303, // Explicitly set ID
                UserId = _testUserId,
                Amount = 75.00m,
                CategoryId = entertainmentCategory.Id,
                WalletId = _testWalletId,
                Note = "Movie tickets",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-3),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 304, // Explicitly set ID
                UserId = _testUserId,
                Amount = 200.00m,
                CategoryId = salaryCategory.Id,
                WalletId = _testWalletId,
                Note = "Monthly salary",
                TransactionType = "Income",
                Timestamp = DateTime.UtcNow.AddDays(-5),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 305, // Explicitly set ID
                UserId = _testUserId,
                Amount = 30.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Old transaction",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-31), // Outside of 30 day window
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 306, // Explicitly set ID
                UserId = _testUserId,
                Amount = 45.00m,
                CategoryId = entertainmentCategory.Id,
                WalletId = _testWalletId,
                Note = "Deleted transaction",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                IsDeleted = true, // Deleted transaction
            },
            new PersonalTransaction
            {
                Id = 307, // Explicitly set ID
                UserId = "different-user-id",
                Amount = 100.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Other user's transaction",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                IsDeleted = false,
            },
        };

        await _dbContext.PersonalTransactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act
        var (categories, grandTotal) = await _repository.GetReportOnCategories(
            _testUserId,
            30,
            null
        );

        // Assert
        Assert.Equal(425.00m, grandTotal); // Sum of non-deleted transactions within 30 days

        var categoriesDict = categories.ToDictionary(c => c.Category.Title, c => c.Total);
        Assert.Equal(3, categoriesDict.Count);
        Assert.Equal(150.00m, categoriesDict["Groceries"]);
        Assert.Equal(75.00m, categoriesDict["Entertainment"]);
        Assert.Equal(200.00m, categoriesDict["Salary"]);
    }

    [Fact]
    public async Task GetReportOnCategories_ShouldFilterByType()
    {
        // Arrange
        var groceryCategory = new Category
        {
            Id = 2,
            Title = "Groceries",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var entertainmentCategory = new Category
        {
            Id = 3,
            Title = "Entertainment",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var salaryCategory = new Category
        {
            Id = 4,
            Title = "Salary",
            UserId = _testUserId,
            DefaultTransactionType = "Income",
            IsDeleted = false,
        };
        var bonusCategory = new Category
        {
            Id = 5,
            Title = "Bonus",
            UserId = _testUserId,
            DefaultTransactionType = "Income",
            IsDeleted = false,
        };

        _dbContext.Categories.AddRange(
            groceryCategory,
            entertainmentCategory,
            salaryCategory,
            bonusCategory
        );
        await _dbContext.SaveChangesAsync();

        var transactions = new[]
        {
            new PersonalTransaction
            {
                Id = 401, // Explicitly set ID
                UserId = _testUserId,
                Amount = 100.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Weekly shopping",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 402, // Explicitly set ID
                UserId = _testUserId,
                Amount = 75.00m,
                CategoryId = entertainmentCategory.Id,
                WalletId = _testWalletId,
                Note = "Movie tickets",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-3),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 403, // Explicitly set ID
                UserId = _testUserId,
                Amount = 200.00m,
                CategoryId = salaryCategory.Id,
                WalletId = _testWalletId,
                Note = "Monthly salary",
                TransactionType = "Income",
                Timestamp = DateTime.UtcNow.AddDays(-5),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 404, // Explicitly set ID
                UserId = _testUserId,
                Amount = 300.00m,
                CategoryId = bonusCategory.Id,
                WalletId = _testWalletId,
                Note = "Quarterly bonus",
                TransactionType = "Income",
                Timestamp = DateTime.UtcNow.AddDays(-10),
                IsDeleted = false,
            },
        };

        await _dbContext.PersonalTransactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act - Filter for Expense only
        var (expenseCategories, expenseTotal) = await _repository.GetReportOnCategories(
            _testUserId,
            30,
            "Expense"
        );

        // Assert
        Assert.Equal(175.00m, expenseTotal);

        var expenseCategoriesDict = expenseCategories.ToDictionary(
            c => c.Category.Title,
            c => c.Total
        );
        Assert.Equal(2, expenseCategoriesDict.Count);
        Assert.Equal(100.00m, expenseCategoriesDict["Groceries"]);
        Assert.Equal(75.00m, expenseCategoriesDict["Entertainment"]);
        Assert.DoesNotContain(expenseCategories, c => c.Category.Title == "Salary");
        Assert.DoesNotContain(expenseCategories, c => c.Category.Title == "Bonus");

        // Act - Filter for Income only
        var (incomeCategories, incomeTotal) = await _repository.GetReportOnCategories(
            _testUserId,
            30,
            "Income"
        );

        // Assert
        Assert.Equal(500.00m, incomeTotal);

        var incomeCategoriesDict = incomeCategories.ToDictionary(
            c => c.Category.Title,
            c => c.Total
        );
        Assert.Equal(2, incomeCategoriesDict.Count);
        Assert.Equal(200.00m, incomeCategoriesDict["Salary"]);
        Assert.Equal(300.00m, incomeCategoriesDict["Bonus"]);
        Assert.DoesNotContain(incomeCategories, c => c.Category.Title == "Groceries");
        Assert.DoesNotContain(incomeCategories, c => c.Category.Title == "Entertainment");
    }

    [Fact]
    public async Task GetReportOnCategories_ShouldRespectDaysParameter()
    {
        // Arrange
        var groceryCategory = new Category
        {
            Id = 2,
            Title = "Groceries",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var entertainmentCategory = new Category
        {
            Id = 3,
            Title = "Entertainment",
            UserId = _testUserId,
            DefaultTransactionType = "Expense",
            IsDeleted = false,
        };
        var salaryCategory = new Category
        {
            Id = 4,
            Title = "Salary",
            UserId = _testUserId,
            DefaultTransactionType = "Income",
            IsDeleted = false,
        };

        _dbContext.Categories.AddRange(groceryCategory, entertainmentCategory, salaryCategory);
        await _dbContext.SaveChangesAsync();

        var transactions = new[]
        {
            new PersonalTransaction
            {
                Id = 501, // Explicitly set ID
                UserId = _testUserId,
                Amount = 100.00m,
                CategoryId = groceryCategory.Id,
                WalletId = _testWalletId,
                Note = "Weekly shopping",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 502, // Explicitly set ID
                UserId = _testUserId,
                Amount = 75.00m,
                CategoryId = entertainmentCategory.Id,
                WalletId = _testWalletId,
                Note = "Movie tickets",
                TransactionType = "Expense",
                Timestamp = DateTime.UtcNow.AddDays(-8),
                IsDeleted = false,
            },
            new PersonalTransaction
            {
                Id = 503, // Explicitly set ID
                UserId = _testUserId,
                Amount = 200.00m,
                CategoryId = salaryCategory.Id,
                WalletId = _testWalletId,
                Note = "Monthly salary",
                TransactionType = "Income",
                Timestamp = DateTime.UtcNow.AddDays(-15),
                IsDeleted = false,
            },
        };

        await _dbContext.PersonalTransactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act - Last 7 days
        var (categoriesWeek, totalWeek) = await _repository.GetReportOnCategories(
            _testUserId,
            7,
            null
        );

        // Assert
        Assert.Equal(100.00m, totalWeek);

        var weekCategoriesDict = categoriesWeek.ToDictionary(c => c.Category.Title, c => c.Total);
        Assert.Single(weekCategoriesDict);
        Assert.Equal(100.00m, weekCategoriesDict["Groceries"]);
        Assert.DoesNotContain(categoriesWeek, c => c.Category.Title == "Entertainment");
        Assert.DoesNotContain(categoriesWeek, c => c.Category.Title == "Salary");

        // Act - Last 10 days
        var (categoriesTenDays, totalTenDays) = await _repository.GetReportOnCategories(
            _testUserId,
            10,
            null
        );

        // Assert
        Assert.Equal(175.00m, totalTenDays);

        var tenDaysCategoriesDict = categoriesTenDays.ToDictionary(
            c => c.Category.Title,
            c => c.Total
        );
        Assert.Equal(2, tenDaysCategoriesDict.Count);
        Assert.Equal(100.00m, tenDaysCategoriesDict["Groceries"]);
        Assert.Equal(75.00m, tenDaysCategoriesDict["Entertainment"]);
        Assert.DoesNotContain(categoriesTenDays, c => c.Category.Title == "Salary");
    }

    [Fact]
    public async Task SaveChangeAsync_ShouldPersistChanges()
    {
        // Arrange
        var transaction = new PersonalTransaction
        {
            Id = 601, // Explicitly set ID
            UserId = _testUserId,
            Amount = 100.50m,
            CategoryId = _testCategoryId,
            WalletId = _testWalletId,
            Note = "Weekly shopping",
            TransactionType = "Expense",
            Timestamp = DateTime.UtcNow,
            IsDeleted = false,
        };

        _dbContext.PersonalTransactions.Add(transaction);
        await _dbContext.SaveChangesAsync();

        // Modify the entity
        transaction.Amount = 150.75m;
        transaction.Note = "Updated note";

        // Act
        var result = await _repository.SaveChangeAsync();

        // Assert
        Assert.Equal(1, result); // One entity modified
        var updatedTransaction = await _dbContext.PersonalTransactions.FindAsync(transaction.Id);
        Assert.NotNull(updatedTransaction);
        Assert.Equal(150.75m, updatedTransaction.Amount);
        Assert.Equal("Updated note", updatedTransaction.Note);
    }

    public void Dispose()
    {
        // Clean up resources after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
