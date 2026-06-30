using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Application.Benefits.Queries.GetActiveBenefits;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/benefits")]
[Authorize(Policy = "EmployeePolicy")]
public class BenefitsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetActiveBenefitsQuery(), cancellationToken);
        return Ok(result);
    }
}
