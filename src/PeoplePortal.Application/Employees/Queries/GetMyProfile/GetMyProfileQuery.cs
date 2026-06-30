using MediatR;
using PeoplePortal.Application.Employees.Dtos;

namespace PeoplePortal.Application.Employees.Queries.GetMyProfile;

public sealed record GetMyProfileQuery(string EmployeeId) : IRequest<EmployeeDto?>;
