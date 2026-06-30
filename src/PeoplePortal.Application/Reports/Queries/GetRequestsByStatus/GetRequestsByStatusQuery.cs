using MediatR;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsByStatus;

public sealed record GetRequestsByStatusQuery() : IRequest<IReadOnlyList<RequestsByStatusDto>>;
