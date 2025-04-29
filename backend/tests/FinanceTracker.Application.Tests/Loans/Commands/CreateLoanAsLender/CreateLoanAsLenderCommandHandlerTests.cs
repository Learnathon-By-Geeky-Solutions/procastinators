using FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;
using FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Loans.Commands.CreateLoan.Tests;

public class CreateLoanAsLenderLCommandHandlerTests
{
    private readonly Mock<ILogger<CreateLoanAsLenderCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly CreateLoanAsLenderCommandHandler _handler;
    private readonly string _userId = "lender-id";
    private readonly int _walletId = 1;

    public CreateLoanAsLenderLCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateLoanAsLenderCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _handler = new CreateLoanAsLenderCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _loanRepositoryMock.Object,
            _walletRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesLoan()
    {
        // Arrange
        var testUser = new UserDto(_userId, "test@example.com");
        var createLoanCommand = new CreateLoanAsLenderCommand
        {
            Amount = 1000,
            DueDate = DateTime.UtcNow.AddMonths(1),
            Note = "Test loan",
            WalletId = _walletId,
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            Balance = 2000,
            IsDeleted = false,
        };

        _userContextMock.Setup(context => context.GetUser()).Returns(testUser);
        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(wallet);

        Loan? capturedLoan = null;
        _loanRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Loan>()))
            .Callback<Loan>(loan => capturedLoan = loan)
            .ReturnsAsync(1);

        // Act
        var resultId = await _handler.Handle(createLoanCommand, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, resultId);

        // Verify wallet balance was decreased
        Xunit.Assert.Equal(1000, wallet.Balance); // 2000 - 1000 = 1000

        Xunit.Assert.NotNull(capturedLoan);
        Xunit.Assert.Equal(_userId, capturedLoan.LenderId);
        Xunit.Assert.Equal(createLoanCommand.Amount, capturedLoan.Amount);
        Xunit.Assert.Equal(createLoanCommand.Note, capturedLoan.Note);
        Xunit.Assert.Equal(createLoanCommand.DueDate, capturedLoan.DueDate);
        Xunit.Assert.Equal(createLoanCommand.Amount, capturedLoan.DueAmount);
        Xunit.Assert.Null(capturedLoan.LoanRequestId);
        Xunit.Assert.False(capturedLoan.IsDeleted);

        // Verify repository method was called
        _loanRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<Loan>()),
            Times.Once,
            "CreateAsync should be called exactly once"
        );
    }

    [Fact]
    public async Task Handle_WalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var testUser = new UserDto(_userId, "test@example.com");
        var createLoanCommand = new CreateLoanAsLenderCommand
        {
            Amount = 1000,
            DueDate = DateTime.UtcNow.AddMonths(1),
            Note = "Test loan",
            WalletId = _walletId,
        };

        _userContextMock.Setup(context => context.GetUser()).Returns(testUser);
        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync((Wallet?)null);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(createLoanCommand, CancellationToken.None)
        );

        // Verify repository method was never called
        _loanRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<Loan>()),
            Times.Never,
            "CreateAsync should not be called when wallet is not found"
        );
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var command = new CreateLoanAsLenderCommand { };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new CreateLoanAsLenderCommand
        {
            WalletId = _walletId,
            Amount = 500m,
            DueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };

        _userContextMock
            .Setup(context => context.GetUser())
            .Returns(new UserDto(_userId, "test@example.com"));

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync((Wallet?)null); // Simulate wallet not found

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithDeletedWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new CreateLoanAsLenderCommand
        {
            WalletId = _walletId,
            Amount = 500m,
            DueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            IsDeleted = true, // Simulate a deleted wallet
            Balance = 1000m,
        };

        _userContextMock
            .Setup(context => context.GetUser())
            .Returns(new UserDto(_userId, "test@example.com"));
        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(wallet);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenWalletDoesNotBelongToUser_ThrowsForbiddenException()
    {
        // Arrange
        var command = new CreateLoanAsLenderCommand
        {
            WalletId = 1,
            Amount = 500m,
            DueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };

        var wallet = new Wallet
        {
            Id = command.WalletId,
            UserId = "different-user-id", // Wallet belongs to a different user
            Balance = 1000m,
            IsDeleted = false,
        };
        _walletRepositoryMock.Setup(repo => repo.GetById(command.WalletId)).ReturnsAsync(wallet);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
