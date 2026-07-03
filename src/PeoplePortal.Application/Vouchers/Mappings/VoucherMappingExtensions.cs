using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Vouchers.Mappings;

public static class VoucherMappingExtensions
{
    public static VoucherDto ToDto(this Voucher voucher) =>
        new(voucher.Id,
            voucher.EmployeeId,
            voucher.Period,
            voucher.NominaType.ToString(),
            voucher.Status.ToString(),
            voucher.FileUrl,
            voucher.Notes,
            voucher.RequestedAt,
            voucher.UpdatedAtUtc);
}
