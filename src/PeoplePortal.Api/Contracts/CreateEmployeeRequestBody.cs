using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateEmployeeRequestBody(
    [property: Required] string KeycloakId,
    [property: Required] string Code,
    [property: Required] string FullName,
    [property: Required] string Email,
    string? Phone,
    [property: Required] string Department,
    [property: Required] string Position,
    [property: Required] DateOnly HireDate,
    [property: Required] string ContractType,
    string? EmergencyContact,
    string? Site,
    string? ManagerId);
