using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Wallets.Dtos.Tests;

public class WalletProfileTests
{
    private IMapper _mapper;

    public WalletProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<WalletProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForWalletToWalletDto_MapsCorrectly()
    {
        // Arrange

        var wallet = new Wallet()
        {
            Id = 1,
            Type = "Bank",
            Name = "name",
            Currency = "BDT",
            Balance = 4000
        };

        // Act

        var walletDto = _mapper.Map<WalletDto>(wallet);

        // Assert

        walletDto.Should().NotBeNull();
        walletDto.Id.Should().Be(wallet.Id);
        walletDto.Type.Should().Be(wallet.Type);
        walletDto.Name.Should().Be(wallet.Name);
        walletDto.Currency.Should().Be(wallet.Currency);
        walletDto.Balance.Should().Be(wallet.Balance);
    }
}