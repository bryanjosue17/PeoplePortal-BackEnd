namespace PeoplePortal.Application.Requests.Dtos;

public sealed record HrRequestDto(
    Guid Id,
    string EmployeeId,
    string Type,
    string Status,
    DateOnly? VacationStartDate,
    DateOnly? VacationEndDate,
    string? CertificateType,
    string? Reason,
    string? HrComment,
    string? ReviewedBy,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);