using MediatR;
using PeoplePortal.Application.Announcements.Dtos;

namespace PeoplePortal.Application.Announcements.Commands.DeactivateAnnouncement;

public sealed record DeactivateAnnouncementCommand(Guid AnnouncementId) : IRequest<AnnouncementDto>;
