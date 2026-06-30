namespace PeoplePortal.Api.Contracts;

public sealed record CreateBenefitRequestBody(
    string Name,
    string Type,
    string? Description);
