using FinanceTracker.Application.Users.Dtos;
using MediatR;

namespace FinanceTracker.Application.Users.Queries.GetUserInfoByEmail;

public class GetUserInfoByEmailQuery : IRequest<UserInfoDto>
{
    public string Email { get; set; } = default!;
}
