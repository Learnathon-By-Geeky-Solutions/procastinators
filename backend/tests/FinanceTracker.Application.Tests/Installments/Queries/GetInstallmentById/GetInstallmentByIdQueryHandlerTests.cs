using AutoMapper;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Installments.Queries.GetAllInstallments;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById.Tests;

public class GetInstallmentByIdQueryHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetInstallmentByIdQueryHandler _handler;
    private readonly int _loanId = 1;
    private readonly int _installmentId = 1;
    private readonly UserDto _user = new("test", "test@test.com");

    public GetInstallmentByIdQueryHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetInstallmentByIdQueryHandler(
            _userContextMock.Object,
            _loanRepositoryMock.Object,
            _installmentRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllInstallmentsForCurrentUser()
    {
        // Arrange
        var query = new GetInstallmentByIdQuery { LoanId = _loanId, Id = _installmentId };

        var installment = new Installment
        {
            Id = _installmentId,
            Amount = 100,
            Timestamp = DateTime.Now,
            LoanId = _loanId,
            NextDueDate = DateTime.Now.AddDays(30),
        };

        var expectedDto = new InstallmentDto()
        {
            Id = _installmentId,
            Amount = 100,
            Timestamp = DateTime.Now,
            LoanId = _loanId,
            NextDueDate = DateTime.Now.AddDays(30),
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanId, _user.Id))
            .ReturnsAsync(new Loan { Id = _loanId, LenderId = _user.Id });

        _installmentRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanId, _installmentId))
            .ReturnsAsync(installment);

        _mapperMock.Setup(m => m.Map<InstallmentDto>(installment)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<InstallmentDto>(installment), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var query = new GetInstallmentByIdQuery { };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(query, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_ForInstallmentNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var installmentId = 1;
        var command = new GetInstallmentByIdQuery() { Id = installmentId };

        _installmentRepositoryMock
            .Setup(r => r.GetByIdAsync(_loanId, installmentId))
            .ReturnsAsync((Installment?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
