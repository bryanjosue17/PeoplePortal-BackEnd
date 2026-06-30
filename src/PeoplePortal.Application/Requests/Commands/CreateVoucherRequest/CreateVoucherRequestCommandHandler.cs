using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Requests.Commands.CreateVoucherRequest;

public sealed class CreateVoucherRequestCommandHandler(IHrRequestRepository repository)
    : IRequestHandler<CreateVoucherRequestCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(CreateVoucherRequestCommand request, CancellationToken cancellationToken)
    {
        HrRequest entity = HrRequest.CreateVoucher(request.EmployeeId, request.Period, request.Reason);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
