using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IBenefitRepository
{
    Task<IReadOnlyList<Benefit>> GetActiveAsync(CancellationToken cancellationToken);
    Task<Benefit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Benefit>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Benefit benefit, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
