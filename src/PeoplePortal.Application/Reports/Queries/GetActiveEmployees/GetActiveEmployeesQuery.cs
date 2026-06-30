using MediatR;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetActiveEmployees;

public sealed record GetActiveEmployeesQuery() : IRequest<ActiveEmployeesDto>;
