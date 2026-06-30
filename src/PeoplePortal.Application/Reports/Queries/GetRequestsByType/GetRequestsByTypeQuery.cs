using MediatR;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsByType;

public sealed record GetRequestsByTypeQuery() : IRequest<IReadOnlyList<RequestsByTypeDto>>;
