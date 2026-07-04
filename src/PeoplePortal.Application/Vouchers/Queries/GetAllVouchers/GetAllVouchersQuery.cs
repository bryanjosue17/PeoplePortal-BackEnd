using MediatR;
using PeoplePortal.Application.Vouchers.Dtos;

namespace PeoplePortal.Application.Vouchers.Queries.GetAllVouchers;

public sealed record GetAllVouchersQuery : IRequest<IReadOnlyList<VoucherDto>>;
