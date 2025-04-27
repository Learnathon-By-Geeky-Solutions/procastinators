using AutoMapper;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Loans.Dtos.LoanDTO.Tests;

public class LoanProfileTests
{
    private IMapper _mapper;

    public LoanProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LoanProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForLoanToLoanDto_MapsCorrectly()
    {
        // Arrange
        var loan = new Loan()
        {
            Id = 1,
            Amount = 1000,
            Note = "Test Loan",
            IssuedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(30),
            LenderId = "1",
            DueAmount = 100,
        };

        // Act
        var loanDto = _mapper.Map<LoanDto>(loan);

        // Assert
        loanDto.Should().NotBeNull();
        loanDto.Id.Should().Be(loan.Id);
        loanDto.Amount.Should().Be(loan.Amount);
        loanDto.Note.Should().Be(loan.Note);
        loanDto.IssuedAt.Should().Be(loan.IssuedAt);
        loanDto.DueDate.Should().Be(loan.DueDate);
        loanDto.LenderId.Should().Be(loan.LenderId);
        loanDto.DueAmount.Should().Be(loan.DueAmount);
    }
}
