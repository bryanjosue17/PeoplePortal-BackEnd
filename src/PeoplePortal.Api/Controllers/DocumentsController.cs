using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Documents.Queries.GetMyDocuments;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/documents")]
[Authorize(Policy = "EmployeePolicy")]
public class DocumentsController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var employeeId = User.GetRequiredUserId();
        var result = await mediator.Send(new GetMyDocumentsQuery(employeeId), cancellationToken);
        return Ok(result);
    }
}
