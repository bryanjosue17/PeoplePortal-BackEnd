namespace PeoplePortal.Api.Contracts;

public sealed record UpdateEmployeeRequestBody(
    string Department,
    string Position,
    string ContractType,
    string Status);
