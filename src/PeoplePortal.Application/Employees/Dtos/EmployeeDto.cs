namespace PeoplePortal.Application.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string KeycloakId,
    string Code,
    string FullName,
    string Email,
    string? Phone,
    string Department,
    string Position,
    DateOnly HireDate,
    string ContractType,
    string Status,
    string? EmergencyContact,
    string? Site,
    string? ManagerId,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
