namespace PeoplePortal.Application.Vouchers.Dtos;

public sealed record VoucherDto(
    Guid Id,
    string EmployeeId,
    string Period,
    string Status,
    string? FileUrl,
    string? Reason,
    DateTime RequestedAt,
    DateTime? UpdatedAtUtc);
