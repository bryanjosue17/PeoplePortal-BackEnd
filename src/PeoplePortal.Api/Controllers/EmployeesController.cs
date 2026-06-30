using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Employees.Commands.CreateEmployee;
using PeoplePortal.Application.Employees.Commands.UpdateEmployee;
using PeoplePortal.Application.Employees.Commands.UpdateMyProfile;
using PeoplePortal.Application.Employees.Queries.GetAllEmployees;
using PeoplePortal.Application.Employees.Queries.GetEmployeeById;
using PeoplePortal.Application.Employees.Queries.GetMyProfile;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Policy = "EmployeePolicy")]
public class EmployeesController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetMyProfileQuery(employeeId), cancellationToken);
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequestBody body, CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(
            new UpdateMyProfileCommand(employeeId, body.Phone, body.EmergencyContact, body.Site),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("~/api/hr/employees")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("~/api/hr/employees/{id:guid}")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("~/api/hr/employees")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequestBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateEmployeeCommand(body.KeycloakId, body.Code, body.FullName, body.Email, body.Phone,
                body.Department, body.Position, body.HireDate, body.ContractType, body.EmergencyContact, body.Site, body.ManagerId),
            cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("~/api/hr/employees/{id:guid}")]
    [Authorize(Policy = "HrPolicy")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequestBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UpdateEmployeeCommand(id, body.Department, body.Position, body.ContractType, body.Status),
            cancellationToken);
        return Ok(result);
    }
}
