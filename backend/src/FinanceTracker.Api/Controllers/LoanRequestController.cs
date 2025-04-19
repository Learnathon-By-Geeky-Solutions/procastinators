using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;
using FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;
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
        return CreatedAtAction(nameof(GetLoanRequestByIdQuery), new { id }, null);
    }

}
