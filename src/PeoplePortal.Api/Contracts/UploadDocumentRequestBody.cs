namespace PeoplePortal.Api.Contracts;

public sealed record UploadDocumentRequestBody(
    string EmployeeId,
    string Name,
    string Type,
    string FileUrl,
    DateOnly? ExpiresAt);
