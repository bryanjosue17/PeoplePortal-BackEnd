using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Application.Vouchers.Mappings;

namespace PeoplePortal.Application.Vouchers.Queries.GetMyVouchers;

public sealed class GetMyVouchersQueryHandler(IVoucherRepository repository)
    : IRequestHandler<GetMyVouchersQuery, IReadOnlyList<VoucherDto>>
{
    public async Task<IReadOnlyList<VoucherDto>> Handle(GetMyVouchersQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
