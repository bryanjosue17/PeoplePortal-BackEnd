namespace PeoplePortal.Api.Contracts;

public sealed record CreateVoucherForEmployeeBody(string EmployeeId, string Period, string? Reason);
public sealed record UploadVoucherFileBody(string FileUrl);
