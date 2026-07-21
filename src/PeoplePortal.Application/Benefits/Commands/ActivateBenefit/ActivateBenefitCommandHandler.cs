using MediatR;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Benefits.Commands.ActivateBenefit;

public sealed class ActivateBenefitCommandHandler(IBenefitRepository repository)
    : IRequestHandler<ActivateBenefitCommand>
{
    public async Task Handle(ActivateBenefitCommand request, CancellationToken cancellationToken)
    {
        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Benefit with id {request.Id} was not found.");

        benefit.Activate();
        await repository.SaveChangesAsync(cancellationToken);
    }
}
