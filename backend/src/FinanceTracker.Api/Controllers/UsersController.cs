using FinanceTracker.Application.Users.Queries.GetUserInfoByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUserInfoByEmail([FromRoute] string email)
    {
        var result = await mediator.Send(new GetUserInfoByEmailQuery { Email = email });
        return Ok(result);
    }
}
