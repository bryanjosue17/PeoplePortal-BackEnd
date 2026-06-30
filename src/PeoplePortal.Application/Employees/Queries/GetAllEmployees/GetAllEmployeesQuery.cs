using MediatR;
using PeoplePortal.Application.Employees.Dtos;

namespace PeoplePortal.Application.Employees.Queries.GetAllEmployees;

public sealed record GetAllEmployeesQuery() : IRequest<IReadOnlyList<EmployeeDto>>;
