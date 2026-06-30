using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Employee?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Employee>> GetByManagerIdAsync(string managerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Employee employee, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
