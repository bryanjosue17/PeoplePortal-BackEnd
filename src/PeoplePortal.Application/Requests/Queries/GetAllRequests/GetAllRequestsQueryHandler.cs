using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;

namespace PeoplePortal.Application.Requests.Queries.GetAllRequests;

public sealed class GetAllRequestsQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetAllRequestsQuery, IReadOnlyList<HrRequestDto>>
{
    public async Task<IReadOnlyList<HrRequestDto>> Handle(GetAllRequestsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
