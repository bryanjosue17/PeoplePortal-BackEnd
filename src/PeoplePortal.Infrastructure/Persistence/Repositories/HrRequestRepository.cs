using Microsoft.EntityFrameworkCore;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence.Repositories;

public class HrRequestRepository(PeoplePortalDbContext dbContext) : IHrRequestRepository
{
    public async Task AddAsync(HrRequest request, CancellationToken cancellationToken)
    {
        await dbContext.HrRequests.AddAsync(request, cancellationToken);
    }

    public async Task<HrRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.HrRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<HrRequest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.HrRequests
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<HrRequest>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken)
    {
        return await dbContext.HrRequests
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<HrRequest>> GetByManagerIdAsync(string managerId, CancellationToken cancellationToken)
    {
        return await dbContext.HrRequests
            .Where(x => x.ReviewedBy == managerId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}