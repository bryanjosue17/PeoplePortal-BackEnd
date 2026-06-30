using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;

namespace PeoplePortal.Application.Requests.Commands.CancelRequest;

public sealed class CancelRequestCommandHandler(IHrRequestRepository repository)
    : IRequestHandler<CancelRequestCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(CancelRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Request '{request.RequestId}' not found.");

        entity.Cancel(request.EmployeeId);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
