using MediatR;
using PeoplePortal.Application.Announcements.Dtos;

namespace PeoplePortal.Application.Announcements.Commands.CreateAnnouncement;

public sealed record CreateAnnouncementCommand(
    string Title,
    string Body,
    string Type,
    string CreatedBy,
    DateTime? ExpiresAt) : IRequest<AnnouncementDto>;
