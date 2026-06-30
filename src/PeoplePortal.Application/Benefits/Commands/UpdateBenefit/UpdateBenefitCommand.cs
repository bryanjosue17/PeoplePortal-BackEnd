using MediatR;
using PeoplePortal.Application.Benefits.Dtos;

namespace PeoplePortal.Application.Benefits.Commands.UpdateBenefit;

public sealed record UpdateBenefitCommand(
    Guid Id,
    string Name,
    string? Description) : IRequest<BenefitDto>;
