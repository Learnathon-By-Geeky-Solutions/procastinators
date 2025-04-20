using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Commands.DeletePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById;
using FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories;
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
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPersonalTransactionsQuery query)
    {
        var transactions = await mediator.Send(query);
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var transaction = await mediator.Send(new GetPersonalTransactionByIdQuery { Id = id });
        return Ok(transaction);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdatePersonalTransactionCommand command
    )
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeletePersonalTransactionCommand { Id = id });
        return NoContent();
    }

    [HttpGet("report/categories")]
    public async Task<IActionResult> GetReportOnCategories(
        [FromQuery] GetReportOnCategoriesQuery query
    )
    {
        var report = await mediator.Send(query);
        return Ok(report);
    }
}
