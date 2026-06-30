using MediatR;
using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Application.Benefits.Mappings;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Benefits.Queries.GetAllBenefits;

public sealed class GetAllBenefitsQueryHandler(IBenefitRepository repository)
    : IRequestHandler<GetAllBenefitsQuery, IReadOnlyList<BenefitDto>>
{
    public async Task<IReadOnlyList<BenefitDto>> Handle(GetAllBenefitsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
