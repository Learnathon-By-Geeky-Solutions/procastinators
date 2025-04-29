using AutoMapper;
using FinanceTracker.Application.Users.Dtos;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Users.Queries.GetUserInfoByEmail;

public class GetUserInfoByEmailQueryHandler(
    ILogger<GetUserInfoByEmailQueryHandler> logger,
    IUserContext userContext,
    IUserRepository userRepo,
    IMapper mapper
) : IRequestHandler<GetUserInfoByEmailQuery, UserInfoDto>
{
    public async Task<UserInfoDto> Handle(
        GetUserInfoByEmailQuery request,
        CancellationToken cancellationToken
    )
    {
        var reqUser = userContext.GetUser() ?? throw new ForbiddenException();
        logger.LogInformation("{@ru} requested:\n{@r}", reqUser, request);

        var user =
            await userRepo.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException(nameof(User), request.Email);

        return mapper.Map<UserInfoDto>(user);
    }
}
