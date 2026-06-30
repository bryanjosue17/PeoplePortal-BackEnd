namespace PeoplePortal.Api.Contracts;

public sealed record CreateVoucherRequestBody(
    string Period,
    string? Reason);
