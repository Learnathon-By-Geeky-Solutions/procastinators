using FinanceTracker.Application.Wallets.Dtos;
using MediatR;

namespace FinanceTracker.Application.Wallets.Queries.GetAllWallets;

public class GetAllWalletsQuery : IRequest<IEnumerable<WalletDto>>
{

}
