using Microsoft.EntityFrameworkCore;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence.Repositories;

public class BenefitRepository(PeoplePortalDbContext dbContext) : IBenefitRepository
{
    public async Task<IReadOnlyList<Benefit>> GetActiveAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Benefits
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Benefit?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Benefits.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Benefit>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Benefits
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Benefit benefit, CancellationToken cancellationToken)
    {
        await dbContext.Benefits.AddAsync(benefit, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
