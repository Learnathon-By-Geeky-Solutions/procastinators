using FinanceTracker.Application.Wallets.Dtos;
using MediatR;

namespace FinanceTracker.Application.Wallets.Queries.GetWalletById;

public class GetWalletByIdQuery : IRequest<WalletDto>
{
    public int Id { get; set; }
}
