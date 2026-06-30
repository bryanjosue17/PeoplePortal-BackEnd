using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateVoucherRequestBody(
    [property: Required] string Period,
    string? Reason);
