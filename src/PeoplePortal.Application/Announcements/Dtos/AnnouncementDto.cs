namespace PeoplePortal.Application.Announcements.Dtos;

public sealed record AnnouncementDto(
    Guid Id,
    string Title,
    string Body,
    string Type,
    DateTime PublishedAt,
    DateTime? ExpiresAt,
    string CreatedBy,
    bool IsActive);
