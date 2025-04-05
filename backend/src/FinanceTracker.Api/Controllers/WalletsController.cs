using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Application.Wallets.Commands.DeleteWallet;
using FinanceTracker.Application.Wallets.Commands.TransferFund;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Application.Wallets.Queries.GetWalletById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetAllWalletsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetWalletByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWalletCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateWalletCommand command
    )
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteWalletCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/transfer")]
    public async Task<IActionResult> Transfer(
        [FromRoute] int id,
        [FromBody] TransferFundCommand command
    )
    {
        command.SourceWalletId = id;
        await mediator.Send(command);
        return NoContent();
    }
}
