using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly FinanceTrackerDbContext _dbContext;
        private readonly CategoryRepository _repository;
        private readonly string _testUserId = "test-user-id";

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging() // Enable sensitive data logging
                .Options;

            _dbContext = new FinanceTrackerDbContext(options);
            _repository = new CategoryRepository(_dbContext);
        }

        [Fact]
        public async Task Create_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var category = new Category
            {
                Title = "Groceries",
                UserId = _testUserId,
                IsDeleted = false,
                DefaultTransactionType = "Expense",
            };

            // Act
            var createdId = await _repository.Create(category);

            // Assert
            Assert.True(createdId > 0);
            var savedCategory = await _dbContext.Categories.FindAsync(createdId);
            Assert.NotNull(savedCategory);
            Assert.Equal("Groceries", savedCategory.Title);
            Assert.Equal(_testUserId, savedCategory.UserId);
            Assert.False(savedCategory.IsDeleted);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOnlyUserCategories()
        {
            // Arrange
            var categories = new[]
            {
                new Category
                {
                    Title = "Entertainment",
                    UserId = _testUserId,
                    IsDeleted = false,
                    DefaultTransactionType = "Expense",
                },
                new Category
                {
                    Title = "Food",
                    UserId = _testUserId,
                    IsDeleted = false,
                    DefaultTransactionType = "Expense",
                },
                new Category
                {
                    Title = "Other User Category",
                    UserId = "different-user",
                    IsDeleted = false,
                    DefaultTransactionType = "Income",
                },
                new Category
                {
                    Title = "Deleted Category",
                    UserId = _testUserId,
                    IsDeleted = true,
                    DefaultTransactionType = "Income",
                },
            };

            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAll(_testUserId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Title == "Entertainment");
            Assert.Contains(result, c => c.Title == "Food");
            Assert.DoesNotContain(result, c => c.Title == "Other User Category");
            Assert.DoesNotContain(result, c => c.Title == "Deleted Category");
        }

        [Fact]
        public async Task GetById_ShouldReturnCategory_WhenExists()
        {
            // Arrange
            var category = new Category
            {
                Title = "Bills",
                UserId = _testUserId,
                IsDeleted = false,
                DefaultTransactionType = "Income",
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            var categoryId = category.Id;

            // Act
            var result = await _repository.GetById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Bills", result.Title);
            Assert.Equal(_testUserId, result.UserId);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Act
            var result = await _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnDeletedCategory()
        {
            // Arrange
            var category = new Category
            {
                Title = "Income",
                UserId = _testUserId,
                IsDeleted = true,
                DefaultTransactionType = "Income",
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            var categoryId = category.Id;

            // Act
            var result = await _repository.GetById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            // Arrange
            var category = new Category
            {
                Title = "Savings",
                UserId = _testUserId,
                IsDeleted = false,
                DefaultTransactionType = "Income",
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            // Modify the entity
            category.Title = "Updated Savings";
            category.IsDeleted = true;

            // Act
            var result = await _repository.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result); // One entity modified
            var updatedCategory = await _dbContext.Categories.FindAsync(category.Id);
            Assert.NotNull(updatedCategory);
            Assert.Equal("Updated Savings", updatedCategory.Title);
            Assert.True(updatedCategory.IsDeleted);
        }

        public void Dispose()
        {
            // Clean up resources after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
