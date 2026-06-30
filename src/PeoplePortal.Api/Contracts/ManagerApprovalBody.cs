using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record ManagerApprovalBody(
    [property: Required, StringLength(20)] string Status,
    string? HrComment);
