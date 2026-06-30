namespace PeoplePortal.Api.Contracts;

public sealed record CreateVacationRequestBody(
    DateOnly StartDate,
    DateOnly EndDate,
    string? Reason,
    string? ManagerId);