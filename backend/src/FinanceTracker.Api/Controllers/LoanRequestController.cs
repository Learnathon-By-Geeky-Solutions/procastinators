using FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;
using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;
using FinanceTracker.Application.LoanRequests.Queries.GetAllSentLoanRequest;
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
    public async Task<IActionResult> ApproveLoanRequest(int id, [FromBody] int lenderWalletId)
    {
        await mediator.Send(new ApproveLoanRequestCommand(id, lenderWalletId));
        return NoContent();
    }

    [HttpGet("received")]
    public async Task<IActionResult> GetAllReceivedLoanRequests()
    {
        var result = await mediator.Send(new GetAllReceivedLoanRequestQuery());
        return Ok(result);
    }

    [HttpGet("sent")]
    public async Task<IActionResult> GetAllSentLoanRequests()
    {
        var result = await mediator.Send(new GetAllSentLoanRequestQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanRequestById(int id)
    {
        var result = await mediator.Send(new GetLoanRequestByIdQuery(id));
        return Ok(result);
    }
}
