using MediatR;
using PeoplePortal.Application.Employees.Dtos;

namespace PeoplePortal.Application.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
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
    string? ManagerId) : IRequest<EmployeeDto>;
