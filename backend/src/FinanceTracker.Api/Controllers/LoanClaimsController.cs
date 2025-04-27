using FinanceTracker.Application.LoanClaims.Commands;
using FinanceTracker.Application.LoanClaims.Queries.GetAllLoanClaims;
using FinanceTracker.Application.LoanClaims.Queries.GetLoanClaimById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanClaimsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLoanClaims()
    {
        var result = await mediator.Send(new GetAllLoanClaimsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanClaimById(int id)
    {
        var result = await mediator.Send(new GetLoanClaimByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("{id}/claim")]
    public async Task<IActionResult> ClaimFund(
        [FromRoute] int id,
        [FromBody] ClaimFundCommand command
    )
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
