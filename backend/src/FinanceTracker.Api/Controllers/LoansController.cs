using FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;
using FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsLender;
using FinanceTracker.Application.Loans.Queries.GetLoanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController(IMediator mediator) : ControllerBase
{
    [HttpPost("lend")]
    public async Task<IActionResult> CreateLoanAsLender(CreateLoanAsLenderCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetLoanById), new { id }, null);
    }

    [HttpPost("borrow")]
    public async Task<IActionResult> CreateLoanAsBorrower(CreateLoanAsBorrowerCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetLoanById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(int id)
    {
        var result = await mediator.Send(new GetLoanByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("lent")]
    public async Task<IActionResult> GetAllLoansAsLender([FromQuery] GetAllLoansAsLenderQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("borrowed")]
    public async Task<IActionResult> GetAllLoansAsBorrower(
        [FromQuery] GetAllLoansAsBorrowerQuery query
    )
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
