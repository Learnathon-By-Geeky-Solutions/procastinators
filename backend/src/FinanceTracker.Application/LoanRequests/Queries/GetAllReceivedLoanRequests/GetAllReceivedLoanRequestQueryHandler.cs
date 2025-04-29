using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;

public class GetAllReceivedLoanRequestQueryHandler(
    ILogger<GetAllReceivedLoanRequestQueryHandler> logger,
    ILoanRequestRepository repo,
    IUserContext userContext,
    IMapper mapper
) : IRequestHandler<GetAllReceivedLoanRequestQuery, IEnumerable<LoanRequestDto>>
{
    public async Task<IEnumerable<LoanRequestDto>> Handle(
        GetAllReceivedLoanRequestQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var list = await repo.GetAllReceivedAsync(user.Id);
        logger.LogInformation("list: {@r}", list);
        return mapper.Map<IEnumerable<LoanRequestDto>>(list);
    }
}
