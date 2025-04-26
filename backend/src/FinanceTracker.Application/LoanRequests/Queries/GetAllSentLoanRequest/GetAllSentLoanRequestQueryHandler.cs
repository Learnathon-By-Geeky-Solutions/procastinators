
using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllSentLoanRequest;

public class GetAllSentLoanRequestQueryHandler(ILogger<GetAllSentLoanRequestQueryHandler> logger,
    ILoanRequestRepository repo,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetAllSentLoanRequestQuery, IEnumerable<LoanRequestDto>>
{
    public async Task<IEnumerable<LoanRequestDto>> Handle(GetAllSentLoanRequestQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var list = await repo.GetAllSentAsync(user!.Id);
        logger.LogInformation("list: {@r}", list);
        return mapper.Map<IEnumerable<LoanRequestDto>>(list);
    }
}
