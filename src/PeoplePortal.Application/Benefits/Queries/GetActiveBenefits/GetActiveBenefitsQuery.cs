using MediatR;
using PeoplePortal.Application.Benefits.Dtos;

namespace PeoplePortal.Application.Benefits.Queries.GetActiveBenefits;

public sealed record GetActiveBenefitsQuery() : IRequest<IReadOnlyList<BenefitDto>>;
