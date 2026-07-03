using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Contracts.Persistence;

public interface IVoucherRepository
{
    Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Voucher>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Voucher>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Voucher voucher, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
