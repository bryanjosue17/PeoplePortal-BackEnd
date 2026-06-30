using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Document>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Document>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Document document, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
