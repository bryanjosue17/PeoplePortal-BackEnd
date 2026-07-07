using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Application.Vouchers.Commands.CreateVoucherForEmployee;
using PeoplePortal.Application.Vouchers.Commands.UploadVoucherFile;
using PeoplePortal.Application.Vouchers.Queries.GetAllVouchers;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/nomina")]
[Authorize(Roles = "nomina,hr,admin")]
public class HrNominaController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllVouchersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNominaBody body, CancellationToken cancellationToken)
    {
        if (body is null)
            return BadRequest(new { error = "Body is required.", detail = "body was null after deserialization" });
        if (string.IsNullOrWhiteSpace(body.EmployeeId))
            return BadRequest(new { error = "EmployeeId is required.", detail = $"EmployeeId='{body.EmployeeId}'" });
        if (string.IsNullOrWhiteSpace(body.Period))
            return BadRequest(new { error = "Period is required.", detail = $"Period='{body.Period}'" });
        if (!Enum.TryParse<NominaType>(body.NominaType, out var nominaType))
            return BadRequest(new { error = $"Invalid NominaType: {body.NominaType}" });
        var result = await mediator.Send(
            new CreateVoucherForEmployeeCommand(body.EmployeeId, body.Period, nominaType, body.Notes),
            cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/upload")]
    public async Task<IActionResult> Upload(Guid id, [FromBody] UploadNominaFileBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UploadVoucherFileCommand(id, body.FileUrl), cancellationToken);
        return Ok(result);
    }
}
