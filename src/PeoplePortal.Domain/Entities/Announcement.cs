using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Domain.Entities;

public class Announcement
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public AnnouncementType Type { get; private set; }
    public DateTime PublishedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Announcement()
    {
    }

    public static Announcement Create(string title, string body, AnnouncementType type, string createdBy, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body is required.", nameof(body));
        if (string.IsNullOrWhiteSpace(createdBy))
            throw new ArgumentException("CreatedBy is required.", nameof(createdBy));

        return new Announcement
        {
            Id = Guid.NewGuid(),
            Title = title,
            Body = body,
            Type = type,
            PublishedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            CreatedBy = createdBy,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
