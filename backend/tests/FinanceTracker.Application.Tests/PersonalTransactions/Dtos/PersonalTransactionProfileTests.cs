using AutoMapper;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Dtos.Tests;

public class PersonalTransactionProfileTests
{
    private IMapper _mapper;

    public PersonalTransactionProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PersonalTransactionProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForPersonalTransactionToPersonalTransactionDto_MapsCorrectly()
    {
        // Arrange

        var personalTransaction = new PersonalTransaction()
        {
            Id = 1,
            TransactionType = "Income",
            Amount = 500,
            Timestamp = DateTime.Parse("2022-12-05 08:30:00"), // Issue: doesn't maps this property should have been mapped
            Note = "Its me the tester haha",
            WalletId = 12,
            CategoryId = 1
        };

        // Act

        var personalTransactionDto = _mapper.Map<PersonalTransactionDto>(personalTransaction);

        // Assert

        personalTransactionDto.Should().NotBeNull();
        personalTransactionDto.Id.Should().Be(personalTransaction.Id);
        personalTransactionDto.TransactionType.Should().Be(personalTransaction.TransactionType);
        personalTransactionDto.Amount.Should().Be(personalTransaction.Amount);
        personalTransactionDto.Timestamp.Should().Be(personalTransaction.Timestamp);
        personalTransactionDto.Note.Should().Be(personalTransaction.Note);
        personalTransactionDto.WalletId.Should().Be(personalTransaction.WalletId);
        personalTransactionDto.CategoryId.Should().Be(personalTransaction.CategoryId);
    }
}

