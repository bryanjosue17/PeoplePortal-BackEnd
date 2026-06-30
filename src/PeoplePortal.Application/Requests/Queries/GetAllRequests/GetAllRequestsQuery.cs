using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Queries.GetAllRequests;

public sealed record GetAllRequestsQuery() : IRequest<IReadOnlyList<HrRequestDto>>;
