using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Application.Vouchers.Mappings;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Vouchers.Commands.CreateVoucherForEmployee;

public sealed class CreateVoucherForEmployeeCommandHandler(IVoucherRepository repository)
    : IRequestHandler<CreateVoucherForEmployeeCommand, VoucherDto>
{
    public async Task<VoucherDto> Handle(CreateVoucherForEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = Voucher.Create(request.EmployeeId, request.Period, request.Reason);
        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }
}
