using MediatR;
using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Application.Benefits.Mappings;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Benefits.Queries.GetActiveBenefits;

public sealed class GetActiveBenefitsQueryHandler(IBenefitRepository repository)
    : IRequestHandler<GetActiveBenefitsQuery, IReadOnlyList<BenefitDto>>
{
    public async Task<IReadOnlyList<BenefitDto>> Handle(GetActiveBenefitsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetActiveAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
