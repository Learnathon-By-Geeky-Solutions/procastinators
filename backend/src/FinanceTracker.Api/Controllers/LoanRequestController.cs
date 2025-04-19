using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;
using FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;
using FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class LoanRequestController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLoanRequest(CreateLoanRequestCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetLoanRequestById), new { id }, null);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveLoanRequest(int id)
    {
        await mediator.Send(new ApproveLoanRequestCommand (id));
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLoanRequests()
    {
        var result = await mediator.Send(new GetAllLoanRequestQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanRequestById(int id)
    {
        var result = await mediator.Send(new GetLoanRequestByIdQuery(id));
        return Ok(result);
    }
}
