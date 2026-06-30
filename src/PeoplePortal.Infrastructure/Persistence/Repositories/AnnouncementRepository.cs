using Microsoft.EntityFrameworkCore;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository(PeoplePortalDbContext dbContext) : IAnnouncementRepository
{
    public async Task<IReadOnlyList<Announcement>> GetActiveAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Announcements
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.PublishedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Announcements
            .OrderByDescending(x => x.PublishedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Announcement announcement, CancellationToken cancellationToken)
    {
        await dbContext.Announcements.AddAsync(announcement, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
