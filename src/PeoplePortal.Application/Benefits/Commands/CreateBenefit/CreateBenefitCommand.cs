using MediatR;
using PeoplePortal.Application.Benefits.Dtos;

namespace PeoplePortal.Application.Benefits.Commands.CreateBenefit;

public sealed record CreateBenefitCommand(
    string Name,
    string Type,
    string? Description) : IRequest<BenefitDto>;
