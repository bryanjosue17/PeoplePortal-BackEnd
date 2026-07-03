using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Vouchers.Queries.GetMyVouchers;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/vouchers")]
[Authorize(Policy = "EmployeePolicy")]
public class VouchersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetMyVouchersQuery(employeeId), cancellationToken);
        return Ok(result);
    }
}
