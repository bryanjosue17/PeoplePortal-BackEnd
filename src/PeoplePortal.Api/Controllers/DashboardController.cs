using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Dashboard.Queries.GetDashboard;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Policy = "EmployeePolicy")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetDashboardQuery(employeeId), cancellationToken);
        return Ok(result);
    }
}
