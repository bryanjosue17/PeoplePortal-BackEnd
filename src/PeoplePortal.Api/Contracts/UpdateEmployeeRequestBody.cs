using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record UpdateEmployeeRequestBody(
    [property: Required] string Department,
    [property: Required] string Position,
    [property: Required] string ContractType,
    [property: Required] string Status);
