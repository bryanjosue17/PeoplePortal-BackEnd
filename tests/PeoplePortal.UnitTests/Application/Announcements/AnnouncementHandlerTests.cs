using FluentAssertions;
using NSubstitute;
using PeoplePortal.Application.Announcements.Commands.CreateAnnouncement;
using PeoplePortal.Application.Announcements.Queries.GetActiveAnnouncements;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Application.Announcements;

public class AnnouncementHandlerTests
{
    [Fact]
    public async Task CreateAnnouncementHandler_ShouldPersistAndReturnDto()
    {
        var repository = Substitute.For<IAnnouncementRepository>();
        var handler = new CreateAnnouncementCommandHandler(repository);
        var command = new CreateAnnouncementCommand(
            "Aviso importante",
            "Recordatorio de pago de nómina este viernes.",
            "News",
            "hr-admin",
            null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Title.Should().Be("Aviso importante");
        result.Body.Should().Be("Recordatorio de pago de nómina este viernes.");
        result.CreatedBy.Should().Be("hr-admin");
        result.IsActive.Should().BeTrue();

        await repository.Received(1).AddAsync(
            Arg.Is<Announcement>(x =>
                x.Title == "Aviso importante" &&
                x.CreatedBy == "hr-admin" &&
                x.IsActive == true),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAnnouncementHandler_WithInvalidType_ShouldThrow()
    {
        var repository = Substitute.For<IAnnouncementRepository>();
        var handler = new CreateAnnouncementCommandHandler(repository);
        var command = new CreateAnnouncementCommand(
            "Titulo",
            "Cuerpo",
            "TipoInvalido",
            "hr-admin",
            null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetActiveAnnouncementsHandler_ShouldReturnMappedDtos()
    {
        var repository = Substitute.For<IAnnouncementRepository>();
        var entities = new List<Announcement>
        {
            Announcement.Create("Aviso 1", "Cuerpo 1", AnnouncementType.News, "admin"),
            Announcement.Create("Aviso 2", "Cuerpo 2", AnnouncementType.HrNotice, "admin")
        };

        repository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(entities.AsReadOnly());

        var handler = new GetActiveAnnouncementsQueryHandler(repository);
        var result = await handler.Handle(new GetActiveAnnouncementsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Aviso 1");
        result[1].Title.Should().Be("Aviso 2");
    }

    [Fact]
    public async Task GetActiveAnnouncementsHandler_WhenNoAnnouncements_ShouldReturnEmpty()
    {
        var repository = Substitute.For<IAnnouncementRepository>();
        repository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Announcement>());

        var handler = new GetActiveAnnouncementsQueryHandler(repository);
        var result = await handler.Handle(new GetActiveAnnouncementsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
