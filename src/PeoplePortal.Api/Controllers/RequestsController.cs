using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Requests.Commands.CreateCertificateRequest;
using PeoplePortal.Application.Requests.Commands.CreateVacationRequest;
using PeoplePortal.Application.Requests.Queries.GetMyRequests;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize(Policy = "EmployeePolicy")]
public class RequestsController(IMediator mediator) : ControllerBase
{
    [HttpPost("vacation")]
    public async Task<IActionResult> CreateVacation([FromBody] CreateVacationRequestBody body, CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(
            new CreateVacationRequestCommand(employeeId, body.StartDate, body.EndDate, body.Reason),
            cancellationToken);

        return CreatedAtAction(nameof(GetMine), new { id = result.Id }, result);
    }

    [HttpPost("certificate")]
    public async Task<IActionResult> CreateCertificate([FromBody] CreateCertificateRequestBody body, CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(
            new CreateCertificateRequestCommand(employeeId, body.CertificateType, body.Reason),
            cancellationToken);

        return CreatedAtAction(nameof(GetMine), new { id = result.Id }, result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetMyRequestsQuery(employeeId), cancellationToken);

        return Ok(result);
    }
}