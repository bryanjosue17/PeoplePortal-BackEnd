using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Queries.GetMyTeamRequests;

public sealed record GetMyTeamRequestsQuery(string ManagerId) : IRequest<IReadOnlyList<HrRequestDto>>;
