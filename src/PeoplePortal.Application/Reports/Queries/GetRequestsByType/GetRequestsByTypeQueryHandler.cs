using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsByType;

public sealed class GetRequestsByTypeQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetRequestsByTypeQuery, IReadOnlyList<RequestsByTypeDto>>
{
    public async Task<IReadOnlyList<RequestsByTypeDto>> Handle(GetRequestsByTypeQuery request, CancellationToken cancellationToken)
    {
        var allRequests = await repository.GetAllAsync(cancellationToken);
        return allRequests
            .GroupBy(x => x.Type)
            .Select(g => new RequestsByTypeDto(g.Key.ToString(), g.Count()))
            .OrderByDescending(x => x.Count)
            .ToArray();
    }
}
