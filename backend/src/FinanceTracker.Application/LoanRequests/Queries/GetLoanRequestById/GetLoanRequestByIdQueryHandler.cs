using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;

public class GetLoanRequestByIdQueryHandler(
    ILogger<GetLoanRequestByIdQueryHandler> logger,
    IUserContext userContext,
    ILoanRequestRepository repo,
    IMapper mapper
) : IRequestHandler<GetLoanRequestByIdQuery, LoanRequestDto>
{
    public async Task<LoanRequestDto> Handle(
        GetLoanRequestByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var entity =
            await repo.GetByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(LoanRequest), request.Id.ToString());

        logger.LogInformation("User: {@u} \n{@r}", user, request);

        return mapper.Map<LoanRequestDto>(entity);
    }
}
