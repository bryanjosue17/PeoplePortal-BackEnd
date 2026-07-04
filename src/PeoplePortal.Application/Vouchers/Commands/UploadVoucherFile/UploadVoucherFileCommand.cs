using MediatR;
using PeoplePortal.Application.Vouchers.Dtos;

namespace PeoplePortal.Application.Vouchers.Commands.UploadVoucherFile;

public sealed record UploadVoucherFileCommand(Guid VoucherId, string FileUrl) : IRequest<VoucherDto>;
