using FinanceTracker.Application.Installments.Commands.PayInstallments;
using FinanceTracker.Application.Installments.Queries.GetInstallmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstallmentController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PayInstallment(PayInstallmentCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetInstallmentById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstallmentById(int id)
    {
        var result = await mediator.Send(new GetInstallmentByIdQuery(id));
        return Ok(result);
    }

}
