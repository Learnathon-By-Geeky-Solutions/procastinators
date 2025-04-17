using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Wallets.Queries.GetWalletById.Tests;

public class GetWalletByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetWalletByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;

    private readonly GetWalletByIdQueryHandler _handler;

    private readonly string _userId = "test-user-id";

    public GetWalletByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetWalletByIdQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _walletRepositoryMock = new Mock<IWalletRepository>();

        _handler = new GetWalletByIdQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _walletRepositoryMock.Object
        );
    }
    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnValidWalletByIdAsync()
    {
        // Arrange
        var walletId = 1;
        var command = new GetWalletByIdQuery()
        {
            Id = walletId,
        };
        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            UserId = _userId
        };
        var expectedDto = new WalletDto
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT"
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);
        _mapperMock.Setup(m => m.Map<WalletDto>(wallet))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<WalletDto>(wallet), Times.Once);
    }
}