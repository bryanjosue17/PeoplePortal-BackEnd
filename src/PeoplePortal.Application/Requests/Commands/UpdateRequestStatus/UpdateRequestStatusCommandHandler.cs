using MediatR;
using PeoplePortal.Application.Common.Interfaces;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Requests.Commands.UpdateRequestStatus;

public sealed class UpdateRequestStatusCommandHandler(IHrRequestRepository repository, IEventBus eventBus)
    : IRequestHandler<UpdateRequestStatusCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Request '{request.RequestId}' not found.");

        entity.SetStatus(request.Status, request.ReviewedBy, request.HrComment);
        await repository.SaveChangesAsync(cancellationToken);

        if (request.Status == RequestStatus.Approved)
        {
            await eventBus.PublishAsync("hr.request.approved", new { entity.Id, entity.EmployeeId, Type = entity.Type.ToString(), entity.UpdatedAtUtc }, cancellationToken);
        }

        return entity.ToDto();
    }
}