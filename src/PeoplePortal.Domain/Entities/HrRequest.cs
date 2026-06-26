using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Domain.Entities;

public class HrRequest
{
    public Guid Id { get; private set; }
    public string EmployeeId { get; private set; } = string.Empty;
    public RequestType Type { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateOnly? VacationStartDate { get; private set; }
    public DateOnly? VacationEndDate { get; private set; }
    public string? CertificateType { get; private set; }
    public string? Reason { get; private set; }
    public string? HrComment { get; private set; }
    public string? ReviewedBy { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private HrRequest()
    {
    }

    public static HrRequest CreateVacation(string employeeId, DateOnly startDate, DateOnly endDate, string? reason)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            throw new ArgumentException("EmployeeId is required.", nameof(employeeId));
        }

        if (endDate < startDate)
        {
            throw new ArgumentException("Vacation end date must be greater or equal than start date.");
        }

        return new HrRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Type = RequestType.Vacation,
            Status = RequestStatus.Submitted,
            VacationStartDate = startDate,
            VacationEndDate = endDate,
            Reason = reason,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static HrRequest CreateCertificate(string employeeId, string certificateType, string? reason)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            throw new ArgumentException("EmployeeId is required.", nameof(employeeId));
        }

        if (string.IsNullOrWhiteSpace(certificateType))
        {
            throw new ArgumentException("Certificate type is required.", nameof(certificateType));
        }

        return new HrRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Type = RequestType.Certificate,
            Status = RequestStatus.Submitted,
            CertificateType = certificateType.Trim(),
            Reason = reason,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void SetStatus(RequestStatus status, string reviewedBy, string? hrComment)
    {
        if (status is not (RequestStatus.Approved or RequestStatus.Rejected))
        {
            throw new ArgumentException("Status can only be Approved or Rejected.", nameof(status));
        }

        if (string.IsNullOrWhiteSpace(reviewedBy))
        {
            throw new ArgumentException("ReviewedBy is required.", nameof(reviewedBy));
        }

        Status = status;
        ReviewedBy = reviewedBy;
        HrComment = hrComment;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}