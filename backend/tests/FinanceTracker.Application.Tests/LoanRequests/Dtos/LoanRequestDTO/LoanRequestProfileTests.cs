using AutoMapper;
using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO.Tests;

public class LoanRequestProfileTests
{
    private IMapper _mapper;

    public LoanRequestProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LoanRequestProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForCreateLoanRequestCommandToLoanRequest_MapsCorrectly()
    {
        // Arrange
        var command = new CreateLoanRequestCommand()
        {
            Amount = 1000,
            Note = "Test Loan Request",
            DueDate = DateTime.UtcNow.AddDays(30),
            LenderId = "1",
        };

        // Act
        var loanRequest = _mapper.Map<Domain.Entities.LoanRequest>(command);

        // Assert
        loanRequest.Should().NotBeNull();
        loanRequest.Amount.Should().Be(command.Amount);
        loanRequest.Note.Should().Be(command.Note);
        loanRequest.DueDate.Should().Be(command.DueDate);
        loanRequest.LenderId.Should().Be(command.LenderId);
    }

    [Fact()]
    public void CreateMap_ForLoanRequestToLoanRequestDto_MapsCorrectly()
    {
        // Arrange
        var loanRequest = new LoanRequest()
        {
            Id = 1,
            Amount = 1000,
            Note = "Test Loan Request",
            DueDate = DateTime.UtcNow.AddDays(30),
            LenderId = "2",
            BorrowerId = "3",
            IsApproved = false,
        };

        // Act
        var loanRequestDto = _mapper.Map<LoanRequestDto>(loanRequest);

        // Assert
        loanRequestDto.Should().NotBeNull();
        loanRequestDto.Id.Should().Be(loanRequest.Id);
        loanRequestDto.Amount.Should().Be(loanRequest.Amount);
        loanRequestDto.Note.Should().Be(loanRequest.Note);
        loanRequestDto.DueDate.Should().Be(loanRequest.DueDate);
        loanRequestDto.LenderId.Should().Be(loanRequest.LenderId);
        loanRequestDto.BorrowerId.Should().Be(loanRequest.BorrowerId);
        loanRequestDto.IsApproved.Should().Be(loanRequest.IsApproved);
    }
}
