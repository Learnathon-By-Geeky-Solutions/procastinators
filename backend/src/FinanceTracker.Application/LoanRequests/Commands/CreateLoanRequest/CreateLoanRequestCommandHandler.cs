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
    IMapper mapper,
    IUserStore<User> userStore
) : IRequestHandler<CreateLoanRequestCommand, int>
{
    public async Task<int> Handle(
        CreateLoanRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        var lender =
            await userStore.FindByIdAsync(request.LenderId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.LenderId);

        if (lender.Id == user.Id)
        {
            throw new BadRequestException("Self loan requests are not allowed.");
        }

        var loanRequest = mapper.Map<CreateLoanRequestCommand, LoanRequest>(request);
        loanRequest.BorrowerId = user.Id;

        logger.LogInformation("Creating LoanRequest: {@loanRequest}", loanRequest);

        return await repo.CreateAsync(loanRequest);
    }
}
