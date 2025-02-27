using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }

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
