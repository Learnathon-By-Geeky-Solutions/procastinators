using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Installments.Queries.GetAllInstallments;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById.Tests;

public class GetInstallmentByIdQueryHandlerTests
{
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetInstallmentByIdQueryHandler _handler;
    private readonly int _loanId = 1;
    private readonly int _installmentId = 1;

    public GetInstallmentByIdQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();

        _handler = new GetInstallmentByIdQueryHandler(
            _installmentRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllInstallmentsForCurrentUser()
    {
        // Arrange
        var query = new GetInstallmentByIdQuery(_installmentId);

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

        _installmentRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_installmentId))
            .ReturnsAsync(installment);

        _mapperMock.Setup(m => m.Map<InstallmentDto>(installment)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<InstallmentDto>(installment), Times.Once);
    }

    [Fact]
    public async Task Handle_ForInstallmentNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var installmentId = 1;
        var command = new GetInstallmentByIdQuery(installmentId) { Id = installmentId };

        _installmentRepositoryMock
            .Setup(r => r.GetByIdAsync(installmentId))
            .ReturnsAsync((Installment?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
