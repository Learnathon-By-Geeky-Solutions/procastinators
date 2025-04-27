using AutoMapper;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Installments.Dtos.Tests;

public class InstallmentProfileTests
{
    private Mapper _mapper;

    public InstallmentProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InstallmentProfile>();
        });
        _mapper = new Mapper(configuration);
    }

    [Fact()]
    public void CreateMap_ForInstallmentsToInstallmentDto_MapsCorrectly()
    {
        // Arrange
        var installment = new Installment()
        {
            Id = 1,
            Amount = 100,
            LoanId = 2,
            Timestamp = DateTime.UtcNow,
            NextDueDate = DateTime.Now.AddDays(1),
            Note = "Test Installment",
        };

        // Act
        var installmentDto = _mapper.Map<InstallmentDto>(installment);

        // Assert

        installmentDto.Should().NotBeNull();
        installmentDto.Id.Should().Be(installment.Id);
        installmentDto.Amount.Should().Be(installment.Amount);
        installmentDto.LoanId.Should().Be(installment.LoanId);
        installmentDto.Timestamp.Should().Be(installment.Timestamp);
        installmentDto.NextDueDate.Should().Be(installment.NextDueDate);
        installmentDto.Note.Should().Be(installment.Note);
    }
}
