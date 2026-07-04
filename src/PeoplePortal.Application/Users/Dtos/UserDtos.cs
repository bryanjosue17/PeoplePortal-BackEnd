namespace PeoplePortal.Application.Users.Dtos;

public sealed record KeycloakUserDto(
    string   Id,
    string?  Username,
    string?  Email,
    string?  FirstName,
    string?  LastName,
    bool     Enabled,
    long?    CreatedTimestamp);

public sealed record KeycloakRoleDto(
    string Id,
    string Name,
    string? Description);

public sealed record UserProfileDto(
    string   KeycloakId,
    string?  Username,
    string?  Email,
    string?  FirstName,
    string?  LastName,
    bool     Enabled,
    long?    CreatedTimestamp,
    IReadOnlyList<string> Roles,
    // Linked employee (nullable)
    Guid?    EmployeeId,
    string?  EmployeeCode,
    string?  EmployeeFullName,
    string?  Department,
    string?  Position,
    string?  EmployeeStatus);
