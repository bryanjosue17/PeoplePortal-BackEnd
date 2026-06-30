namespace PeoplePortal.Api.Contracts;

public sealed record CreateEmployeeRequestBody(
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
