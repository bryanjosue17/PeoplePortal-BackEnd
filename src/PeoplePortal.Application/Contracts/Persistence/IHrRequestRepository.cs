using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IHrRequestRepository
{
    Task AddAsync(HrRequest request, CancellationToken cancellationToken);
    Task<HrRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<HrRequest>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HrRequest>> GetByManagerIdAsync(string managerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HrRequest>> GetAllAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}