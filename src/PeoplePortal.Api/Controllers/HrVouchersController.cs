using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Application.Vouchers.Commands.CreateVoucherForEmployee;
using PeoplePortal.Application.Vouchers.Commands.UploadVoucherFile;
using PeoplePortal.Application.Vouchers.Queries.GetAllVouchers;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/vouchers")]
[Authorize(Roles = "nomina,hr,admin")]
public class HrVouchersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllVouchersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoucherForEmployeeBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateVoucherForEmployeeCommand(body.EmployeeId, body.Period, body.Reason),
            cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/upload")]
    public async Task<IActionResult> Upload(Guid id, [FromBody] UploadVoucherFileBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UploadVoucherFileCommand(id, body.FileUrl), cancellationToken);
        return Ok(result);
    }
}
