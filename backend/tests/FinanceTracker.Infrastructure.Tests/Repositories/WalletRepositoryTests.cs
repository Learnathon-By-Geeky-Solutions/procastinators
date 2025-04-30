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
    public class WalletRepositoryTests : IDisposable
    {
        private readonly FinanceTrackerDbContext _dbContext;
        private readonly WalletRepository _repository;
        private readonly string _testUserId = "test-user-id";

        public WalletRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new FinanceTrackerDbContext(options);
            _repository = new WalletRepository(_dbContext);
        }

        [Fact]
        public async Task Create_ShouldAddWalletToDatabase()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Cash Wallet",
                Balance = 1000.50m,
                UserId = _testUserId,
                IsDeleted = false,
                Currency = "USD",
                Type = "Cash",
            };

            // Act
            var createdId = await _repository.Create(wallet);

            // Assert
            Assert.True(createdId > 0);
            var savedWallet = await _dbContext.Wallets.FindAsync(createdId);
            Assert.NotNull(savedWallet);
            Assert.Equal("Cash Wallet", savedWallet.Name);
            Assert.Equal(1000.50m, savedWallet.Balance);
            Assert.Equal(_testUserId, savedWallet.UserId);
            Assert.Equal("USD", savedWallet.Currency);
            Assert.False(savedWallet.IsDeleted);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOnlyUserWallets()
        {
            // Arrange
            var wallets = new[]
            {
                new Wallet
                {
                    Name = "Bank Account",
                    Balance = 5000m,
                    UserId = _testUserId,
                    IsDeleted = false,
                    Currency = "USD",
                    Type = "Bank",
                },
                new Wallet
                {
                    Name = "Credit Card",
                    Balance = -200m,
                    UserId = _testUserId,
                    IsDeleted = false,
                    Currency = "USD",
                    Type = "CreditCard",
                },
                new Wallet
                {
                    Name = "Other User Wallet",
                    Balance = 300m,
                    UserId = "different-user",
                    IsDeleted = false,
                    Currency = "EUR",
                    Type = "Cash",
                },
                new Wallet
                {
                    Name = "Deleted Wallet",
                    Balance = 0m,
                    UserId = _testUserId,
                    IsDeleted = true,
                    Currency = "USD",
                    Type = "Savings",
                },
            };

            await _dbContext.Wallets.AddRangeAsync(wallets);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAll(_testUserId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, w => w.Name == "Bank Account");
            Assert.Contains(result, w => w.Name == "Credit Card");
            Assert.DoesNotContain(result, w => w.Name == "Other User Wallet");
            Assert.DoesNotContain(result, w => w.Name == "Deleted Wallet");
        }

        [Fact]
        public async Task GetById_ShouldReturnWallet_WhenExists()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Savings",
                Balance = 10000m,
                UserId = _testUserId,
                IsDeleted = false,
                Currency = "USD",
                Type = "Savings",
            };

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();
            var walletId = wallet.Id;

            // Act
            var result = await _repository.GetById(walletId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(walletId, result.Id);
            Assert.Equal("Savings", result.Name);
            Assert.Equal(10000m, result.Balance);
            Assert.Equal(_testUserId, result.UserId);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenWalletDoesNotExist()
        {
            // Act
            var result = await _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnDeletedWallet()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Old Wallet",
                Balance = 0m,
                UserId = _testUserId,
                IsDeleted = true,
                Currency = "USD",
                Type = "Cash",
            };

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();
            var walletId = wallet.Id;

            // Act
            var result = await _repository.GetById(walletId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(walletId, result.Id);
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Investment",
                Balance = 20000m,
                UserId = _testUserId,
                IsDeleted = false,
                Currency = "USD",
                Type = "Investment",
            };

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();

            // Modify the entity
            wallet.Name = "Updated Investment";
            wallet.Balance = 25000m;
            wallet.IsDeleted = true;

            // Act
            var result = await _repository.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result); // One entity modified
            var updatedWallet = await _dbContext.Wallets.FindAsync(wallet.Id);
            Assert.NotNull(updatedWallet);
            Assert.Equal("Updated Investment", updatedWallet.Name);
            Assert.Equal(25000m, updatedWallet.Balance);
            Assert.True(updatedWallet.IsDeleted);
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
