using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Announcements.Commands.CreateAnnouncement;
using PeoplePortal.Application.Announcements.Commands.DeactivateAnnouncement;
using PeoplePortal.Application.Announcements.Queries.GetActiveAnnouncements;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/announcements")]
[Authorize(Policy = "EmployeePolicy")]
public class AnnouncementsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetActiveAnnouncementsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("~/api/hr/announcements")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementRequestBody body, CancellationToken cancellationToken)
    {
        var createdBy = User.GetRequiredUserId();
        var result = await mediator.Send(
            new CreateAnnouncementCommand(body.Title, body.Body, body.Type, createdBy,
                body.ExpiresAt?.ToDateTime(TimeOnly.MinValue)),
            cancellationToken);
        return CreatedAtAction(nameof(GetActive), new { id = result.Id }, result);
    }

    [HttpPatch("~/api/hr/announcements/{id:guid}/deactivate")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeactivateAnnouncementCommand(id), cancellationToken);
        return Ok(result);
    }
}
