using MediatR;
using PeoplePortal.Application.Announcements.Dtos;

namespace PeoplePortal.Application.Announcements.Queries.GetActiveAnnouncements;

public sealed record GetActiveAnnouncementsQuery() : IRequest<IReadOnlyList<AnnouncementDto>>;
