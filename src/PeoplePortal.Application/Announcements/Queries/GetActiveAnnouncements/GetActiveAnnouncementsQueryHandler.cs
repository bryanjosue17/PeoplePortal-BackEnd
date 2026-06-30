using MediatR;
using PeoplePortal.Application.Announcements.Dtos;
using PeoplePortal.Application.Announcements.Mappings;
using PeoplePortal.Application.Contracts.Persistence;

namespace PeoplePortal.Application.Announcements.Queries.GetActiveAnnouncements;

public sealed class GetActiveAnnouncementsQueryHandler(IAnnouncementRepository repository)
    : IRequestHandler<GetActiveAnnouncementsQuery, IReadOnlyList<AnnouncementDto>>
{
    public async Task<IReadOnlyList<AnnouncementDto>> Handle(GetActiveAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetActiveAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
