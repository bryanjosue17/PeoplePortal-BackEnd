namespace PeoplePortal.Application.Employees.Dtos;

public sealed record UpdateProfileDto(
    string? Phone,
    string? EmergencyContact,
    string? Site);
