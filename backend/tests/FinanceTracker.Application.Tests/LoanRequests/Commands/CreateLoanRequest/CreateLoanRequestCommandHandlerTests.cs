using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest.Tests;

public class CreateLoanRequestCommandHandlerTests
{
    private readonly Mock<ILogger<CreateLoanRequestCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepositoryMock;
    private readonly Mock<IUserStore<User>> _userStore;
    private readonly CreateLoanRequestCommandHandler _handler;
    private readonly string _userId = "user-id";

    public CreateLoanRequestCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateLoanRequestCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _loanRequestRepositoryMock = new Mock<ILoanRequestRepository>();
        _userStore = new Mock<IUserStore<User>>();

        _handler = new CreateLoanRequestCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _loanRequestRepositoryMock.Object,
            _mapperMock.Object,
            _userStore.Object
        );
    }

    [Fact()]
    public async Task Handle_ValidRequest_CreatesLoanRequest()
    {
        // Arrange
        var testUser = new UserDto(_userId, "test@example.com");
        var createLoanCommand = new CreateLoanRequestCommand
        {
            LenderId = "lender-id",
            Amount = 1000,
            DueDate = DateTime.UtcNow.AddMonths(1),
            Note = "Test loan request",
        };
        var expectedLoanRequest = new LoanRequest
        {
            LenderId = "lender-id",
            Amount = 1000,
            DueDate = createLoanCommand.DueDate,
            Note = "Test loan request",
            BorrowerId = _userId,
            IsApproved = false,
        };

        _userContextMock.Setup(context => context.GetUser()).Returns(testUser);
        _mapperMock
            .Setup(mapper => mapper.Map<CreateLoanRequestCommand, LoanRequest>(createLoanCommand))
            .Returns(expectedLoanRequest);

        _loanRequestRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<LoanRequest>()))
            .ReturnsAsync(1);

        _userStore
            .Setup(store => store.FindByIdAsync(createLoanCommand.LenderId, CancellationToken.None))
            .ReturnsAsync(new User { Id = createLoanCommand.LenderId });

        // Act
        var resultId = await _handler.Handle(createLoanCommand, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, resultId);

        _loanRequestRepositoryMock.Verify(
            repo =>
                repo.CreateAsync(
                    It.Is<LoanRequest>(request =>
                        request.LenderId == createLoanCommand.LenderId
                        && request.Amount == createLoanCommand.Amount
                        && request.DueDate == createLoanCommand.DueDate
                        && request.Note == createLoanCommand.Note
                        && request.BorrowerId == _userId
                        && request.IsApproved == false
                    )
                ),
            Times.Once
        );

        _userContextMock.Verify(context => context.GetUser(), Times.Once);

        _mapperMock.Verify(
            mapper => mapper.Map<CreateLoanRequestCommand, LoanRequest>(createLoanCommand),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_NullUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var createLoanCommand = new CreateLoanRequestCommand
        {
            LenderId = "lender-id",
            Amount = 1000,
            DueDate = DateTime.UtcNow.AddMonths(1),
            Note = "Test loan request",
        };

        _userContextMock.Setup(context => context.GetUser()).Returns((UserDto?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(createLoanCommand, CancellationToken.None)
        );

        _loanRequestRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<LoanRequest>()),
            Times.Never
        );
        _mapperMock.Verify(
            mapper =>
                mapper.Map<CreateLoanRequestCommand, LoanRequest>(
                    It.IsAny<CreateLoanRequestCommand>()
                ),
            Times.Never
        );
    }
}
