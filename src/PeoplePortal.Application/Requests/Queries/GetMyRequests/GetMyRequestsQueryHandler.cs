using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;

namespace PeoplePortal.Application.Requests.Queries.GetMyRequests;

public sealed class GetMyRequestsQueryHandler(IHrRequestRepository repository)
    : IRequestHandler<GetMyRequestsQuery, IReadOnlyList<HrRequestDto>>
{
    public async Task<IReadOnlyList<HrRequestDto>> Handle(GetMyRequestsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return entities.Select(x => x.ToDto()).ToList();
    }
}