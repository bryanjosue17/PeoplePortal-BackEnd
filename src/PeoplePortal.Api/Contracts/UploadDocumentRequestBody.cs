using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record UploadDocumentRequestBody(
    [property: Required] string EmployeeId,
    [property: Required] string Name,
    [property: Required] string Type,
    [property: Required] string FileUrl,
    DateOnly? ExpiresAt);
