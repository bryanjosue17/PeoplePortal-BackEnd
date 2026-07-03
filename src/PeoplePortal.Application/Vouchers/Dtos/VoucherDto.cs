namespace PeoplePortal.Application.Vouchers.Dtos;

public sealed record VoucherDto(
    Guid Id,
    string EmployeeId,
    string Period,
    string NominaType,
    string Status,
    string? FileUrl,
    string? Notes,
    DateTime RequestedAt,
    DateTime? UpdatedAtUtc);
