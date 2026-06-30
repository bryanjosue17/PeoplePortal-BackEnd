using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Application.Reports.Queries.GetActiveEmployees;
using PeoplePortal.Application.Reports.Queries.GetPendingDocuments;
using PeoplePortal.Application.Reports.Queries.GetRequestsByStatus;
using PeoplePortal.Application.Reports.Queries.GetRequestsByType;
using PeoplePortal.Application.Reports.Queries.GetRequestsOverTime;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/reports")]
[Authorize(Policy = "HrPolicy")]
public class ReportsController(IMediator mediator) : ControllerBase
{
    [HttpGet("requests-by-status")]
    public async Task<IActionResult> GetRequestsByStatus(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRequestsByStatusQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("requests-by-type")]
    public async Task<IActionResult> GetRequestsByType(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRequestsByTypeQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("requests-over-time")]
    public async Task<IActionResult> GetRequestsOverTime(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRequestsOverTimeQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("active-employees")]
    public async Task<IActionResult> GetActiveEmployees(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetActiveEmployeesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("pending-documents")]
    public async Task<IActionResult> GetPendingDocuments(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPendingDocumentsQuery(), cancellationToken);
        return Ok(result);
    }
}
