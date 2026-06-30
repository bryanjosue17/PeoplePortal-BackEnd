using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record UpdateRequestStatusBody(
    [property: Required, StringLength(20)] string Status,
    string? HrComment);