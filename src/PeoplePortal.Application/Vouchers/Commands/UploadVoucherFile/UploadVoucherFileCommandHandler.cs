using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Application.Vouchers.Mappings;

namespace PeoplePortal.Application.Vouchers.Commands.UploadVoucherFile;

public sealed class UploadVoucherFileCommandHandler(IVoucherRepository repository)
    : IRequestHandler<UploadVoucherFileCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(UploadVoucherFileCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.VoucherId, cancellationToken)
            ?? throw new KeyNotFoundException($"Voucher '{request.VoucherId}' not found.");

        entity.Upload(request.FileUrl);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }
}
