using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinanceTracker.Application.Users;

public record UserDto(string Id, string Email);

public interface IUserContext
{
    UserDto? GetUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserDto? GetUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;

        if (user == null) throw new InvalidOperationException("User context is not available");
        if (user.Identity == null || !user.Identity.IsAuthenticated) return null;

        var userId = user.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(claim => claim.Type == ClaimTypes.Email)!.Value;

        return new UserDto(userId, email);
    }
}
