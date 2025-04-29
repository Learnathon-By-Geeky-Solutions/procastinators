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
    public class InstallmentRepositoryTests : IDisposable
    {
        private readonly FinanceTrackerDbContext _dbContext;
        private readonly InstallmentRepository _repository;
        private readonly string _testUserId = "test-user-id";
        private int _testLoanId;

        public InstallmentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new FinanceTrackerDbContext(options);
            _repository = new InstallmentRepository(_dbContext);

            // Create a test loan for use in the tests
            SetupTestLoan().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private async Task SetupTestLoan()
        {
            var loan = new Loan
            {
                LenderId = _testUserId,
                BorrowerId = "borrower-id",
                Amount = 1000,
                LoanRequestId = null,
                Note = "Test Loan Description",
            };

            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();
            _testLoanId = loan.Id;
        }
    }
}
