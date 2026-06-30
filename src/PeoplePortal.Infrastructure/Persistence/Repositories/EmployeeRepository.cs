using Microsoft.EntityFrameworkCore;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence.Repositories;

public class EmployeeRepository(PeoplePortalDbContext dbContext) : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Employee?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken)
    {
        return await dbContext.Employees.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetByManagerIdAsync(string managerId, CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .Where(x => x.ManagerId == managerId)
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Employees
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        await dbContext.Employees.AddAsync(employee, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
