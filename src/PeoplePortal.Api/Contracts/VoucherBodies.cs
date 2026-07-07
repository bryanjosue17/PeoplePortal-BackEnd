namespace PeoplePortal.Api.Contracts;

public sealed record CreateNominaBody(
    string EmployeeId,
    string Period,
    string NominaType = "ComprobanteDepago",
    string? Notes = null);

public sealed record UploadNominaFileBody(string FileUrl);
