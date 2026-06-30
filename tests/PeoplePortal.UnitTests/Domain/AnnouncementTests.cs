using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Domain;

public class AnnouncementTests
{
    [Fact]
    public void Create_ShouldSetActiveAndPublished()
    {
        var announcement = Announcement.Create("Title", "Body", AnnouncementType.News, "admin-1");

        announcement.Title.Should().Be("Title");
        announcement.Body.Should().Be("Body");
        announcement.Type.Should().Be(AnnouncementType.News);
        announcement.CreatedBy.Should().Be("admin-1");
        announcement.IsActive.Should().BeTrue();
        announcement.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Deactivate_ShouldSetInactive()
    {
        var announcement = Announcement.Create("Title", "Body", AnnouncementType.News, "admin-1");

        announcement.Deactivate();

        announcement.IsActive.Should().BeFalse();
    }
}
