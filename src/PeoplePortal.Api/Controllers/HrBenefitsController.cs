using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Application.Benefits.Commands.CreateBenefit;
using PeoplePortal.Application.Benefits.Commands.DeactivateBenefit;
using PeoplePortal.Application.Benefits.Commands.UpdateBenefit;
using PeoplePortal.Application.Benefits.Queries.GetAllBenefits;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/benefits")]
[Authorize(Policy = "HrPolicy")]
public class HrBenefitsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllBenefitsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBenefitRequestBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateBenefitCommand(body.Name, body.Type, body.Description),
            cancellationToken);
        return CreatedAtAction(null, new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBenefitRequestBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UpdateBenefitCommand(id, body.Name, body.Description),
            cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeactivateBenefitCommand(id), cancellationToken);
        return NoContent();
    }
}
