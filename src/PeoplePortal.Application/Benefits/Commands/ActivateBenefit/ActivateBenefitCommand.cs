using MediatR;

namespace PeoplePortal.Application.Benefits.Commands.ActivateBenefit;

public sealed record ActivateBenefitCommand(Guid Id) : IRequest;
