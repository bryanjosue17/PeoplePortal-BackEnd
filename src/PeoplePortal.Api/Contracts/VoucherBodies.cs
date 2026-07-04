using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateNominaBody(
    string EmployeeId,
    string Period,
    NominaType NominaType = NominaType.ComprobanteDepago,
    string? Notes = null);

public sealed record UploadNominaFileBody(string FileUrl);
