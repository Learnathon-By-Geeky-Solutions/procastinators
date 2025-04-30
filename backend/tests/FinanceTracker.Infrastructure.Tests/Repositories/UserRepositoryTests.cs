using System;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTracker.Tests.Infrastructure.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly FinanceTrackerDbContext _dbContext;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new FinanceTrackerDbContext(options);
            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var testEmail = "test@example.com";
            var user = new User
            {
                Id = "1",
                Email = testEmail,
                PasswordHash = "hashedPassword123",
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(testEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEmail, result.Email);
            Assert.Equal("1", result.Id);
            Assert.Equal("hashedPassword123", result.PasswordHash);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentEmail = "nonexistent@example.com";

            // Act
            var result = await _repository.GetByEmailAsync(nonExistentEmail);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCorrectUser_WhenMultipleUsersExist()
        {
            // Arrange
            var testEmail = "test@example.com";
            var users = new[]
            {
                new User
                {
                    Email = "user1@example.com",
                    Id = "1",
                    PasswordHash = "hash1",
                },
                new User
                {
                    Email = testEmail,
                    Id = "2",
                    PasswordHash = "hash2",
                },
                new User
                {
                    Email = "user3@example.com",
                    Id = "3",
                    PasswordHash = "hash3",
                },
            };

            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(testEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEmail, result.Email);
            Assert.Equal("2", result.Id);
            Assert.Equal("hash2", result.PasswordHash);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldBeCaseInsensitive()
        {
            // Arrange
            var testEmail = "Test@Example.com";
            var user = new User
            {
                Email = "test@example.com", // Note the different case
                Id = "Test User",
                PasswordHash = "hashedPassword123",
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(testEmail);

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
}
