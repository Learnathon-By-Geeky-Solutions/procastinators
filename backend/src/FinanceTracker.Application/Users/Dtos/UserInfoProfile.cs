using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Users.Dtos;

public class UserInfoProfile : AutoMapper.Profile
{
    public UserInfoProfile()
    {
        CreateMap<User, UserInfoDto>();
    }
}
