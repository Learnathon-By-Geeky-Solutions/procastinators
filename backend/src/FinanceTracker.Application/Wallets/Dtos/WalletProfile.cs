using AutoMapper;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Wallets.Dtos;

public class WalletProfile: Profile
{
    public WalletProfile()
    {
        CreateMap<CreateWalletCommand, Wallet>();
    }
}
