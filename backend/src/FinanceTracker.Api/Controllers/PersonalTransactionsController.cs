using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonalTransactionsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonalTransactionCommand command)
    {
        var id = await mediator.Send(command);
        //return CreatedAtAction(nameof(GetById), new { id }, null);
        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPersonalTransactionsQuery query)
    {
        var transactions = await mediator.Send(query);
        return Ok(transactions);
    }
}
