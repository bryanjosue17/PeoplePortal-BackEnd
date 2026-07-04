using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Requests.Commands.ApproveByManager;
using PeoplePortal.Application.Requests.Queries.GetMyTeamRequests;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/manager/requests")]
[Authorize(Policy = "ManagerPolicy")]
public class ManagerController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTeamRequests(CancellationToken cancellationToken)
    {
        var managerId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetMyTeamRequestsQuery(managerId), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ManagerApprovalBody body, CancellationToken cancellationToken)
    {
        var managerId = User.GetRequiredUserId();
        var result = await mediator.Send(
            new ApproveByManagerCommand(id, managerId, body.HrComment),
            cancellationToken);
        return Ok(result);
    }
}
