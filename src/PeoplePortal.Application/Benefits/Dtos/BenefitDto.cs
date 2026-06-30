namespace PeoplePortal.Application.Benefits.Dtos;

public sealed record BenefitDto(
    Guid Id,
    string Name,
    string? Description,
    string Type,
    bool IsActive);
