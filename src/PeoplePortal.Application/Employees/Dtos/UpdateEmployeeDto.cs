namespace PeoplePortal.Application.Employees.Dtos;

public sealed record UpdateEmployeeDto(
    string Department,
    string Position,
    string ContractType,
    string Status);
