using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Requests.Commands.UpdateRequestStatus;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/requests")]
[Authorize(Policy = "HrPolicy")]
public class HrRequestsController(IMediator mediator) : ControllerBase
{
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRequestStatusBody body, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<RequestStatus>(body.Status, true, out var status))
        {
            return BadRequest("Invalid status. Use Approved or Rejected.");
        }

        var reviewedBy = User.GetRequiredUserId();

        var result = await mediator.Send(
            new UpdateRequestStatusCommand(id, status, reviewedBy, body.HrComment),
            cancellationToken);

        return Ok(result);
    }
}