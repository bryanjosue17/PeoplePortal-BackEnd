namespace PeoplePortal.Application.Documents.Dtos;

public sealed record DocumentDto(
    Guid Id,
    string EmployeeId,
    string Name,
    string Type,
    string Status,
    string? FileUrl,
    DateOnly? ExpiresAt,
    DateTime UploadedAt,
    string? ReviewedBy);
