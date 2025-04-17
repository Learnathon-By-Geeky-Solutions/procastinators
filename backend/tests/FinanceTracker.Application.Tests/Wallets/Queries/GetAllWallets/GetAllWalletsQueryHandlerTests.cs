using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Tests.Wallets.Queries.GetAllWallets;

public class GetAllWalletsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllWalletsQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly GetAllWalletsQueryHandler _handler;
    private readonly string _userId = "test-user-id";

    public GetAllWalletsQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllWalletsQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _walletRepositoryMock = new Mock<IWalletRepository>();

        _handler = new GetAllWalletsQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _walletRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllWalletsForCurrentUser()
    {
        // Arrange
        var query = new GetAllWalletsQuery();

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var wallets = new List<Wallet>
        {
            new Wallet
            {
                Id = 1,
                Name = "Cash Wallet",
                Type = "Cash",
                Currency = "USD",
                Balance = 500.0m,
                UserId = _userId
            },
            new Wallet
            {
                Id = 2,
                Name = "Bank Account",
                Type = "Bank",
                Currency = "EUR",
                Balance = 1500.0m,
                UserId = _userId
            }
        };

        _walletRepositoryMock.Setup(r => r.GetAll(_userId))
            .ReturnsAsync(wallets);

        var walletDtos = new List<WalletDto>
        {
            new WalletDto
            {
                Id = 1,
                Name = "Cash Wallet",
                Type = "Cash",
                Currency = "USD",
                Balance = 500.0m
            },
            new WalletDto
            {
                Id = 2,
                Name = "Bank Account",
                Type = "Bank",
                Currency = "EUR",
                Balance = 1500.0m
            }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<WalletDto>>(wallets))
            .Returns(walletDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(walletDtos);
        _walletRepositoryMock.Verify(r => r.GetAll(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<WalletDto>>(wallets), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoWallets_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllWalletsQuery();

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var wallets = new List<Wallet>();

        _walletRepositoryMock.Setup(r => r.GetAll(_userId))
            .ReturnsAsync(wallets);

        var walletDtos = new List<WalletDto>();

        _mapperMock.Setup(m => m.Map<IEnumerable<WalletDto>>(wallets))
            .Returns(walletDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _walletRepositoryMock.Verify(r => r.GetAll(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<WalletDto>>(wallets), Times.Once);
    }
}