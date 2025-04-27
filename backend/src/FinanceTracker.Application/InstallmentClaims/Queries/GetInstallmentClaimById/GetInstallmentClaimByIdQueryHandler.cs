using AutoMapper;
using FinanceTracker.Application.InstallmentClaims.Dtos;
using FinanceTracker.Application.InstallmentClaims.Queries.GetInstallmentClaimById;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetLoanClaimById;

public class GetInstallmentClaimByIdQueryHandler(
    IUserContext userContext,
    IInstallmentRepository installmentRepository,
    IMapper mapper
) : IRequestHandler<GetInstallmentClaimByIdQuery, InstallmentClaimDto>
{
    public async Task<InstallmentClaimDto> Handle(
        GetInstallmentClaimByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var installmentClaim =
            await installmentRepository.GetInstallmentClaimByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(InstallmentClaim), request.Id.ToString());
        return mapper.Map<InstallmentClaimDto>(installmentClaim);
    }
}
