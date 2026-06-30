using MediatR;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsOverTime;

public sealed record GetRequestsOverTimeQuery() : IRequest<IReadOnlyList<RequestsOverTimeDto>>;
