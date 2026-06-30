using MediatR;
using PeoplePortal.Application.Benefits.Dtos;

namespace PeoplePortal.Application.Benefits.Queries.GetAllBenefits;

public sealed record GetAllBenefitsQuery() : IRequest<IReadOnlyList<BenefitDto>>;
