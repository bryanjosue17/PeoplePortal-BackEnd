namespace PeoplePortal.Api.Contracts;

public sealed record UpdateBenefitRequestBody(
    string Name,
    string? Description);
