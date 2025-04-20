using FinanceTracker.Application.Loans.Commands.CreateLoan;
using FinanceTracker.Application.Loans.Queries.GetAllLoans;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;
using FinanceTracker.Application.Loans.Queries.GetLoanById;
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
        return CreatedAtAction(nameof(GetLoanById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(int id)
    {
        var result = await mediator.Send(new GetLoanByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLoans([FromQuery] GetAllLoansQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("asBorrower")]
    public async Task<IActionResult> GetAllLoansAsBorrower([FromQuery] GetAllLoansAsBorrowerQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
