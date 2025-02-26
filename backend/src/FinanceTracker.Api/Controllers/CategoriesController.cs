using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Application.Wallets.Queries.GetWalletById;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
    }
}
