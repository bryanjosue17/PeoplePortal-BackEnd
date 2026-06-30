namespace PeoplePortal.Api.Contracts;

public sealed record ManagerApprovalBody(
    string Status,
    string? HrComment);
