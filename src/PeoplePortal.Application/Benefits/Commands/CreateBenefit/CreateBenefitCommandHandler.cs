using MediatR;
using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Application.Benefits.Mappings;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Benefits.Commands.CreateBenefit;

public sealed class CreateBenefitCommandHandler(IBenefitRepository repository)
    : IRequestHandler<CreateBenefitCommand, BenefitDto>
{
    public async Task<BenefitDto> Handle(CreateBenefitCommand request, CancellationToken cancellationToken)
    {
        var benefit = Benefit.Create(request.Name, request.Type, request.Description);
        await repository.AddAsync(benefit, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return benefit.ToDto();
    }
}
