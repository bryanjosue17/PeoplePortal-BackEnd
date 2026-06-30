using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Vouchers.Mappings;

public static class VoucherMappingExtensions
{
    public static VoucherDto ToDto(this Voucher voucher)
    {
        return new VoucherDto(
            voucher.Id,
            voucher.EmployeeId,
            voucher.Period,
            voucher.Status.ToString(),
            voucher.FileUrl,
            voucher.Reason,
            voucher.RequestedAt,
            voucher.UpdatedAtUtc);
    }
}
