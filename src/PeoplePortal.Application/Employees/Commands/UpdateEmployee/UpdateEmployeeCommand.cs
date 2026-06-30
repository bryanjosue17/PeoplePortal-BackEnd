using MediatR;
using PeoplePortal.Application.Employees.Dtos;

namespace PeoplePortal.Application.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string Department,
    string Position,
    string ContractType,
    string Status) : IRequest<EmployeeDto>;
