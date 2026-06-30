namespace PeoplePortal.Api.Contracts;

public sealed record UpdateRequestStatusBody(
    string Status,
    string? HrComment);