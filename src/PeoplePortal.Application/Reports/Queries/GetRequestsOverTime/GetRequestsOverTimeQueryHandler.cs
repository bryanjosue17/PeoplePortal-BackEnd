using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsOverTime;

public sealed class GetRequestsOverTimeQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetRequestsOverTimeQuery, IReadOnlyList<RequestsOverTimeDto>>
{
    public async Task<IReadOnlyList<RequestsOverTimeDto>> Handle(GetRequestsOverTimeQuery request, CancellationToken cancellationToken)
    {
        var allRequests = await repository.GetAllAsync(cancellationToken);
        return allRequests
            .GroupBy(x => new { x.CreatedAtUtc.Year, x.CreatedAtUtc.Month })
            .Select(g => new RequestsOverTimeDto(
                $"{g.Key.Year}-{g.Key.Month:D2}",
                g.Count()))
            .OrderBy(x => x.Period)
            .ToArray();
    }
}
