using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;

namespace PeoplePortal.Application.Requests.Commands.UpdateRequestStatus;

public sealed class UpdateRequestStatusCommandHandler(IHrRequestRepository repository)
    : IRequestHandler<UpdateRequestStatusCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Request '{request.RequestId}' not found.");

        entity.SetStatus(request.Status, request.ReviewedBy, request.HrComment);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}