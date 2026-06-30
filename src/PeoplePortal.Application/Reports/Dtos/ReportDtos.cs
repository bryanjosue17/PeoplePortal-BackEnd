namespace PeoplePortal.Application.Reports.Dtos;

public sealed record RequestsByStatusDto(
    string Status,
    int Count);

public sealed record RequestsByTypeDto(
    string Type,
    int Count);

public sealed record RequestsOverTimeDto(
    string Period,
    int Count);

public sealed record ActiveEmployeesDto(
    int Total,
    int Active,
    int OnLeave,
    int Inactive,
    int Terminated);

public sealed record PendingDocumentsDto(
    Guid EmployeeId,
    string EmployeeName,
    string Department,
    int PendingCount,
    int ExpiredCount);
