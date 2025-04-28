using FinanceTracker.Application.Installments.Commands.PayInstallment;
using FinanceTracker.Application.Installments.Commands.ReceiveInstallment;
using FinanceTracker.Application.Installments.Queries.GetAllInstallments;
using FinanceTracker.Application.Installments.Queries.GetInstallmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/loans/{loanId}/installments")]
[Authorize]
public class InstallmentController(IMediator mediator) : ControllerBase
{
    [HttpPost("pay")]
    public async Task<IActionResult> PayInstallment(
        [FromRoute] int loanId,
        [FromBody] PayInstallmentCommand command
    )
    {
        command.LoanId = loanId;
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetInstallmentById), new { loanId, id }, null);
    }

    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveInstallment(
        [FromRoute] int loanId,
        [FromBody] ReceiveInstallmentCommand command
    )
    {
        command.LoanId = loanId;
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetInstallmentById), new { loanId, id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInstallments([FromRoute] int loanId)
    {
        var result = await mediator.Send(new GetAllInstallmentsQuery { LoanId = loanId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstallmentById([FromRoute] int loanId, [FromRoute] int id)
    {
        var result = await mediator.Send(new GetInstallmentByIdQuery { LoanId = loanId, Id = id });
        return Ok(result);
    }
}
