using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Benefits.Mappings;

public static class BenefitMappingExtensions
{
    public static BenefitDto ToDto(this Benefit benefit)
    {
        return new BenefitDto(
            benefit.Id,
            benefit.Name,
            benefit.Description,
            benefit.Type,
            benefit.IsActive);
    }
}
