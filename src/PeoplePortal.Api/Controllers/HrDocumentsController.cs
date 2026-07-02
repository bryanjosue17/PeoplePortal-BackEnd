using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Api.Extensions;
using PeoplePortal.Application.Documents.Commands.UploadDocument;
using PeoplePortal.Application.Documents.Commands.UpdateDocumentStatus;
using PeoplePortal.Application.Documents.Queries.GetAllDocuments;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/documents")]
[Authorize(Policy = "HrPolicy")]
public class HrDocumentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllDocumentsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] UploadDocumentRequestBody body, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UploadDocumentCommand(body.EmployeeId, body.Name, body.Type, body.FileUrl, body.ExpiresAt),
            cancellationToken);
        return CreatedAtAction(null, new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDocumentStatusBody body, CancellationToken cancellationToken)
    {
        var reviewedBy = User.GetRequiredUserId();
        var result = await mediator.Send(
            new UpdateDocumentStatusCommand(id, body.Status, reviewedBy),
            cancellationToken);
        return Ok(result);
    }
}
