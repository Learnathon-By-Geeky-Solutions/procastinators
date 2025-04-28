using AutoMapper;
using FinanceTracker.Application.InstallmentClaims.Dtos;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetAllLoanClaims;

public class GetAllInstallmentClaimsQueryHandler(
    IUserContext userContext,
    IInstallmentRepository installmentRepository,
    IMapper mapper
) : IRequestHandler<GetAllInstallmentClaimsQuery, IEnumerable<InstallmentClaimDto>>
{
    public async Task<IEnumerable<InstallmentClaimDto>> Handle(
        GetAllInstallmentClaimsQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var installmentClaims = await installmentRepository.GetAllInstallmentClaimsAsync(user.Id);
        return mapper.Map<IEnumerable<InstallmentClaimDto>>(installmentClaims);
    }
}
