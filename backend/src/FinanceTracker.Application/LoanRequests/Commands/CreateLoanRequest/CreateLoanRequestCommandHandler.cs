using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestCommandHandler(
    ILogger<CreateLoanRequestCommandHandler> logger,
    IUserContext userContext,
    ILoanRequestRepository repo,
    IWalletRepository walletRepo,
    IMapper mapper,
    UserManager<User> userManager
) : IRequestHandler<CreateLoanRequestCommand, int>
{
    public async Task<int> Handle(
        CreateLoanRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        var lender =
            await userManager.FindByIdAsync(request.LenderId)
            ?? throw new NotFoundException(nameof(User), request.LenderId);

        if (lender.Id == user.Id)
        {
            throw new BadRequestException("Self loan requests are not allowed.");
        }

        var wallet =
            await walletRepo.GetById(request.WalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());

        if (wallet.UserId != user.Id)
        {
            throw new ForbiddenException();
        }

        var loanRequest = mapper.Map<CreateLoanRequestCommand, LoanRequest>(request);
        loanRequest.BorrowerId = user.Id;

        logger.LogInformation("Creating LoanRequest: {@loanRequest}", loanRequest);

        return await repo.CreateAsync(loanRequest);
    }
}
