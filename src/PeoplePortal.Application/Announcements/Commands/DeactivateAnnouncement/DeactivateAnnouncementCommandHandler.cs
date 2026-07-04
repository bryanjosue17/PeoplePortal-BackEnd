using MediatR;
using PeoplePortal.Application.Announcements.Dtos;
using PeoplePortal.Application.Announcements.Mappings;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Announcements.Commands.DeactivateAnnouncement;

public sealed class DeactivateAnnouncementCommandHandler(IAnnouncementRepository repository)
    : IRequestHandler<DeactivateAnnouncementCommand, AnnouncementDto>
{
    public async Task<AnnouncementDto> Handle(DeactivateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.AnnouncementId, cancellationToken)
            ?? throw new KeyNotFoundException($"Announcement '{request.AnnouncementId}' not found.");

        entity.Deactivate();
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
