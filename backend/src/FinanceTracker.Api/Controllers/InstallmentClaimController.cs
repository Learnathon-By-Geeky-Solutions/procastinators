using FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund;
using FinanceTracker.Application.InstallmentClaims.Queries.GetAllInstallmentClaims;
using FinanceTracker.Application.InstallmentClaims.Queries.GetInstallmentClaimById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstallmentClaimsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllInstallmentClaims()
    {
        var result = await mediator.Send(new GetAllInstallmentClaimsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstallmentClaimById(int id)
    {
        var result = await mediator.Send(new GetInstallmentClaimByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("{id}/claim")]
    public async Task<IActionResult> ClaimInstallmentFund(
        [FromRoute] int id,
        [FromBody] ClaimInstallmentFundCommand command
    )
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
