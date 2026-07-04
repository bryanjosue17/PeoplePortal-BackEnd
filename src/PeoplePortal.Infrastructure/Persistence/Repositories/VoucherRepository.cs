using Microsoft.EntityFrameworkCore;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Infrastructure.Persistence.Repositories;

public class VoucherRepository(PeoplePortalDbContext dbContext) : IVoucherRepository
{
    public async Task AddAsync(Voucher voucher, CancellationToken cancellationToken)
        => await dbContext.Vouchers.AddAsync(voucher, cancellationToken);

    public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Vouchers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Voucher>> GetAllAsync(CancellationToken cancellationToken)
        => await dbContext.Vouchers
            .OrderByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Voucher>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken)
        => await dbContext.Vouchers
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
