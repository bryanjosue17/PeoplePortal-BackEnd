using MediatR;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Benefits.Commands.DeactivateBenefit;

public sealed class DeactivateBenefitCommandHandler(IBenefitRepository repository)
    : IRequestHandler<DeactivateBenefitCommand>
{
    public async Task Handle(DeactivateBenefitCommand request, CancellationToken cancellationToken)
    {
        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Benefit with id {request.Id} was not found.");

        benefit.Deactivate();
        await repository.SaveChangesAsync(cancellationToken);
    }
}
