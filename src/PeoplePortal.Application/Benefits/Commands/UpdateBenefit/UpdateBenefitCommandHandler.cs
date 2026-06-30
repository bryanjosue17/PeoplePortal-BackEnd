using MediatR;
using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Application.Benefits.Mappings;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Benefits.Commands.UpdateBenefit;

public sealed class UpdateBenefitCommandHandler(IBenefitRepository repository)
    : IRequestHandler<UpdateBenefitCommand, BenefitDto>
{
    public async Task<BenefitDto> Handle(UpdateBenefitCommand request, CancellationToken cancellationToken)
    {
        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Benefit with id {request.Id} was not found.");

        benefit.Update(request.Name, request.Description);
        await repository.SaveChangesAsync(cancellationToken);
        return benefit.ToDto();
    }
}
