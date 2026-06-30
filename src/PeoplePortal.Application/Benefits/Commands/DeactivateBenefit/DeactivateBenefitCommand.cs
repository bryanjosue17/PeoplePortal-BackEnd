using MediatR;

namespace PeoplePortal.Application.Benefits.Commands.DeactivateBenefit;

public sealed record DeactivateBenefitCommand(Guid Id) : IRequest;
