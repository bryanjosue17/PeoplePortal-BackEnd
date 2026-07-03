using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;

namespace PeoplePortal.Application.Requests.Queries.GetMyTeamRequests;

public sealed class GetMyTeamRequestsQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetMyTeamRequestsQuery, IReadOnlyList<HrRequestDto>>
{
    public async Task<IReadOnlyList<HrRequestDto>> Handle(GetMyTeamRequestsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByManagerIdAsync(request.ManagerId, cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
