using FinanceTracker.Application.LoanRequests.Commands.CreateLoan;
using FinanceTracker.Application.LoanRequests.Queries.GetLoanById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLoan(CreateLoanCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetLoanByIdQuery), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(int id)
    {
        var result = await mediator.Send(new GetLoanByIdQuery { Id = id });
        return Ok(result);
    }
}
