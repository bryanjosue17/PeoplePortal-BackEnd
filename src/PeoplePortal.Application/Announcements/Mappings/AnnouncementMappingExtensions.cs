using PeoplePortal.Application.Announcements.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Announcements.Mappings;

public static class AnnouncementMappingExtensions
{
    public static AnnouncementDto ToDto(this Announcement announcement)
    {
        return new AnnouncementDto(
            announcement.Id,
            announcement.Title,
            announcement.Body,
            announcement.Type.ToString(),
            announcement.PublishedAt,
            announcement.ExpiresAt,
            announcement.CreatedBy,
            announcement.IsActive);
    }
}
