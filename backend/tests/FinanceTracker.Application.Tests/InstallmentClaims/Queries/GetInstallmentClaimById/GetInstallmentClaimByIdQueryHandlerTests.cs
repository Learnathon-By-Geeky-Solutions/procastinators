using AutoMapper;
using FinanceTracker.Application.InstallmentClaims.Dtos;
using FinanceTracker.Application.InstallmentClaims.Queries.GetInstallmentClaimById;
using FinanceTracker.Application.InstallmentClaims.Queries.GetLoanClaimById;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById.Tests;

public class GetInstallmentClaimByIdQueryHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetInstallmentClaimByIdQueryHandler _handler;
    private readonly int _loanId = 1;
    private readonly int _installmentId = 1;
    private readonly string _userId = "test";
    private readonly UserDto _user = new("test", "test@test.com");

    public GetInstallmentClaimByIdQueryHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetInstallmentClaimByIdQueryHandler(
            _userContextMock.Object,
            _installmentRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllInstallmentsForCurrentUser()
    {
        // Arrange
        var query = new GetInstallmentClaimByIdQuery() { Id = _installmentId };

        var installmentClaim = new InstallmentClaim()
        {
            Id = 1,
            InstallmentId = 2,
            IsClaimed = true,
            ClaimedAt = DateTime.Now,
        };

        var expectedDto = new InstallmentClaimDto()
        {
            Id = 1,
            IsClaimed = true,
            ClaimedAt = DateTime.Now,
        };

        _installmentRepositoryMock
            .Setup(repo => repo.GetInstallmentClaimByIdAsync(_installmentId, _userId))
            .ReturnsAsync(installmentClaim);

        _mapperMock.Setup(m => m.Map<InstallmentClaimDto>(installmentClaim)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<InstallmentClaimDto>(installmentClaim), Times.Once);
    }

    [Fact]
    public async Task Handle_ForInstallmentNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var installmentId = 1;
        var command = new GetInstallmentClaimByIdQuery() { Id = installmentId };

        _installmentRepositoryMock
            .Setup(r => r.GetInstallmentClaimByIdAsync(installmentId, _userId))
            .ReturnsAsync((InstallmentClaim?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
