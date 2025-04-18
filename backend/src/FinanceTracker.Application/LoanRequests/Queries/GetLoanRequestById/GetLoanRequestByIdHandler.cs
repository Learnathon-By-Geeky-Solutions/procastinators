using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequest;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;

public class GetLoanRequestByIdQueryHandler(ILogger<GetLoanRequestByIdQueryHandler> logger,
    IUserContext userContext,
    ILoanRequestRepository repo,
    IMapper mapper) : IRequestHandler<GetLoanRequestByIdQuery, LoanRequestDto>
{
    public async Task<LoanRequestDto> Handle(GetLoanRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var entity = await repo.GetByIdAsync(request.Id);
        logger.LogInformation("User: {@u} \n{@r}", user, request);
        if (entity == null)
            throw new NotFoundException("Loan Request", request.Id.ToString());
        if(entity.BorrowerId == null)
        {
            throw new ForbiddenException();
        }
        return mapper.Map<LoanRequestDto>(entity);
    }
}
