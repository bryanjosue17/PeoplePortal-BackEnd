using MediatR;
using PeoplePortal.Application.Vouchers.Dtos;

namespace PeoplePortal.Application.Vouchers.Queries.GetMyVouchers;

public sealed record GetMyVouchersQuery(string EmployeeId) : IRequest<IReadOnlyList<VoucherDto>>;
