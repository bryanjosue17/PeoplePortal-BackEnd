using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Application.Vouchers.Mappings;

namespace PeoplePortal.Application.Vouchers.Queries.GetAllVouchers;

public sealed class GetAllVouchersQueryHandler(IVoucherRepository repository)
    : IRequestHandler<GetAllVouchersQuery, IReadOnlyList<VoucherDto>>
{
    public async Task<IReadOnlyList<VoucherDto>> Handle(GetAllVouchersQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
