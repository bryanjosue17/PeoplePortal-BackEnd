using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateVacationRequestBody(
    [property: Required] DateOnly StartDate,
    [property: Required] DateOnly EndDate,
    string? Reason,
    string? ManagerId);