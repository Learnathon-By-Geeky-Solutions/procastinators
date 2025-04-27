using AutoMapper;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Installments.Queries.GetAllInstallments.Tests;

public class GetAllInstallmentsQueryHandlerTests
{
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllInstallmentsQueryHandler _handler;
    private readonly int _loanId = 1;

    public GetAllInstallmentsQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();

        _handler = new GetAllInstallmentsQueryHandler(
            _installmentRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllInstallmentsForCurrentUser()
    {
        // Arrange
        var query = new GetAllInstallmentsQuery(_loanId);

        var installments = new List<Installment>
        {
            new Installment
            {
                Id = 1,
                Amount = 100,
                Timestamp = DateTime.Now,
                LoanId = _loanId,
                NextDueDate = DateTime.Now.AddDays(30),
            },
            new Installment
            {
                Id = 2,
                Amount = 200,
                Timestamp = DateTime.Now.AddDays(30),
                LoanId = _loanId,
                NextDueDate = DateTime.Now.AddDays(60),
            },
        };

        var installmentDtos = new List<InstallmentDto>
        {
            new InstallmentDto
            {
                Id = 1,
                Amount = 100,
                Timestamp = DateTime.Now,
                LoanId = _loanId,
                NextDueDate = DateTime.Now.AddDays(30),
            },
            new InstallmentDto
            {
                Id = 2,
                Amount = 200,
                Timestamp = DateTime.Now.AddDays(30),
                LoanId = _loanId,
                NextDueDate = DateTime.Now.AddDays(60),
            },
        };

        _installmentRepositoryMock
            .Setup(repo => repo.GetAllByLoanIdAsync(_loanId))
            .ReturnsAsync(installments);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<InstallmentDto>>(It.IsAny<IEnumerable<Installment>>()))
            .Returns(installmentDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(installmentDtos);
        _installmentRepositoryMock.Verify(repo => repo.GetAllByLoanIdAsync(_loanId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<InstallmentDto>>(installments), Times.Once);
    }
}
