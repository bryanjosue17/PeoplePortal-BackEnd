namespace PeoplePortal.Application.Employees.Dtos;

public sealed record CreateEmployeeDto(
    string KeycloakId,
    string Code,
    string FullName,
    string Email,
    string? Phone,
    string Department,
    string Position,
    DateOnly HireDate,
    string ContractType,
    string? EmergencyContact,
    string? Site,
    string? ManagerId);
