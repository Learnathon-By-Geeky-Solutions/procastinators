using AutoMapper;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequest;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;

public class GetAllLoanRequestQueryHandler(ILogger<GetAllLoanRequestQueryHandler> logger,
    ILoanRequestRepository repo,
    IMapper mapper) : IRequestHandler<GetAllLoanRequestQuery, IEnumerable<LoanRequestDto>>
{
    public async Task<IEnumerable<LoanRequestDto>> Handle(GetAllLoanRequestQuery request, CancellationToken cancellationToken)
    {
        var list = await repo.GetAllAsync();
        logger.LogInformation("list: {@r}",list);
        return mapper.Map<IEnumerable<LoanRequestDto>>(list);
    }
}