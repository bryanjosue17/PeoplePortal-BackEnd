using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public string EmployeeId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public DocumentStatus Status { get; private set; }
    public string? FileUrl { get; private set; }
    public DateOnly? ExpiresAt { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string? ReviewedBy { get; private set; }

    private Document()
    {
    }

    public static Document Create(string employeeId, string name, string type, DateOnly? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException("EmployeeId is required.", nameof(employeeId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type is required.", nameof(type));

        return new Document
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Name = name,
            Type = type,
            Status = DocumentStatus.Pending,
            ExpiresAt = expiresAt,
            UploadedAt = DateTime.UtcNow
        };
    }

    public void Upload(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("FileUrl is required.", nameof(fileUrl));

        FileUrl = fileUrl;
        Status = DocumentStatus.Available;
    }

    public void SetStatus(DocumentStatus status, string? reviewedBy = null)
    {
        if (status is DocumentStatus.Available or DocumentStatus.Expired)
            throw new ArgumentException("Use Upload or mark as Expired for that transition.", nameof(status));

        Status = status;
        ReviewedBy = reviewedBy;
    }
}
