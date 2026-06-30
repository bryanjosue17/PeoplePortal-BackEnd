using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Reports.Dtos;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Reports.Queries.GetRequestsByStatus;

public sealed class GetRequestsByStatusQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetRequestsByStatusQuery, IReadOnlyList<RequestsByStatusDto>>
{
    public async Task<IReadOnlyList<RequestsByStatusDto>> Handle(GetRequestsByStatusQuery request, CancellationToken cancellationToken)
    {
        var allRequests = await repository.GetAllAsync(cancellationToken);
        return allRequests
            .GroupBy(x => x.Status)
            .Select(g => new RequestsByStatusDto(g.Key.ToString(), g.Count()))
            .OrderByDescending(x => x.Count)
            .ToArray();
    }
}
