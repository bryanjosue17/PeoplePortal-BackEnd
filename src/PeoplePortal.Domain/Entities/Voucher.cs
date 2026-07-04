using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Domain.Entities;

public class Voucher
{
    public Guid Id { get; private set; }
    public string EmployeeId { get; private set; } = string.Empty;
    public string Period { get; private set; } = string.Empty;
    public NominaType NominaType { get; private set; }
    public VoucherStatus Status { get; private set; }
    public string? FileUrl { get; private set; }
    public string? Notes { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Voucher() { }

    public static Voucher Create(string employeeId, string period, NominaType nominaType = NominaType.ComprobanteDepago, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("EmployeeId is required.", nameof(employeeId));
        if (string.IsNullOrWhiteSpace(period))
            throw new ArgumentException("Period is required.", nameof(period));

        return new Voucher
        {
            Id         = Guid.NewGuid(),
            EmployeeId = employeeId,
            Period     = period,
            NominaType = nominaType,
            Status     = VoucherStatus.Requested,
            Notes      = notes,
            RequestedAt = DateTime.UtcNow
        };
    }

    public void Upload(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("FileUrl is required.", nameof(fileUrl));
        FileUrl    = fileUrl;
        Status     = VoucherStatus.AvailableForDownload;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetStatus(VoucherStatus status)
    {
        if (Status is VoucherStatus.Completed or VoucherStatus.Rejected)
            throw new InvalidOperationException("Cannot change status of a finalized nomina record.");
        Status       = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        Notes        = notes;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}

