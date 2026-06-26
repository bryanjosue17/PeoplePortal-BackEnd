using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Queries.GetMyRequests;

public sealed record GetMyRequestsQuery(string EmployeeId) : IRequest<IReadOnlyList<HrRequestDto>>;