using MediatR;
using PeoplePortal.Application.Dashboard.Dtos;

namespace PeoplePortal.Application.Dashboard.Queries.GetDashboard;

public sealed record GetDashboardQuery(string EmployeeId) : IRequest<DashboardDto>;
