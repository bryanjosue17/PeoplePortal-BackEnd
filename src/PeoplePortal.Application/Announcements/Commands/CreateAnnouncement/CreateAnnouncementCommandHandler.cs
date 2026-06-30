using MediatR;
using PeoplePortal.Application.Announcements.Dtos;
using PeoplePortal.Application.Announcements.Mappings;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Announcements.Commands.CreateAnnouncement;

public sealed class CreateAnnouncementCommandHandler(IAnnouncementRepository repository)
    : IRequestHandler<CreateAnnouncementCommand, AnnouncementDto>
{
    public async Task<AnnouncementDto> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<AnnouncementType>(request.Type, ignoreCase: true, out var type))
            throw new ArgumentException($"Invalid AnnouncementType: {request.Type}");

        Announcement entity = Announcement.Create(request.Title, request.Body, type, request.CreatedBy, request.ExpiresAt);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
