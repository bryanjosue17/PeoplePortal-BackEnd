using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Requests.Commands.ApproveByManager;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/manager/requests/{id:guid}/status")]
[Authorize(Policy = "ManagerPolicy")]
public class ManagerController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ManagerApprovalBody body, CancellationToken cancellationToken)
    {
        var managerId = User.GetRequiredUserId();
        var result = await mediator.Send(
            new ApproveByManagerCommand(id, managerId, body.HrComment),
            cancellationToken);
        return Ok(result);
    }
}
