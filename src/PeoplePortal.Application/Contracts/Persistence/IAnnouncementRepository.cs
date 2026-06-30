using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IAnnouncementRepository
{
    Task<IReadOnlyList<Announcement>> GetActiveAsync(CancellationToken cancellationToken);
    Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Announcement announcement, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
