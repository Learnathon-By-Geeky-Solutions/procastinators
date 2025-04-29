using AutoMapper;
using Castle.Core.Logging;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Users.Dtos;
using FinanceTracker.Application.Users.Queries.GetUserInfoByEmail;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Loans.Queries.GetLoanById.Tests;

public class GetUserInfoByEmailQueryHandlerTests
{
    private readonly Mock<ILogger<GetUserInfoByEmailQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserInfoByEmailQueryHandler _handler;
    private readonly UserDto _user = new("test", "test@test.com");

    public GetUserInfoByEmailQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetUserInfoByEmailQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetUserInfoByEmailQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllLoansForCurrentUser()
    {
        // Arrange
        var query = new GetUserInfoByEmailQuery() { Email = "test@ts.co" };

        var userInfo = new User()
        {
            Id = "2",
            Email = "test@s.c",
            UserName = "test",
        };

        var expectedDto = new UserInfoDto()
        {
            Id = "2",
            Email = "test@s.c",
            UserName = "test",
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync("test@ts.co"))
            .ReturnsAsync(userInfo);

        _mapperMock.Setup(m => m.Map<UserInfoDto>(userInfo)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<UserInfoDto>(userInfo), Times.Once);
    }

    [Fact]
    public async Task Handle_ForLoanNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new GetUserInfoByEmailQuery() { Email = "test@ts.co" };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync("test@ts.co")).ReturnsAsync((User?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
