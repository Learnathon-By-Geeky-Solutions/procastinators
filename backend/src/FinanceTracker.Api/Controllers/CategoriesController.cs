using FinanceTracker.Application.Categories.Commands;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FinanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
    }
}
