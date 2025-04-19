using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestCommandHandler(
    ILogger<CreateLoanRequestCommandHandler> logger,
    IUserContext userContext,
    ILoanRequestRepository repo,
    IMapper mapper) : IRequestHandler<CreateLoanRequestCommand, int>
{
    public async Task<int> Handle(CreateLoanRequestCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        if (user == null) throw new ForbiddenException();

        var loanRequest = mapper.Map<CreateLoanRequestCommand, LoanRequest>(request);
        loanRequest.BorrowerId = user.Id;
        loanRequest.IsApproved = false;

        logger.LogInformation("Creating LoanRequest: {@loanRequest}", loanRequest);

        return await repo.CreateAsync(loanRequest);
    }
}
