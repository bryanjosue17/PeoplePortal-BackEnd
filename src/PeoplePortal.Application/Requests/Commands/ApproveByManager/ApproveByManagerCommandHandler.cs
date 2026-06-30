using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Requests.Commands.ApproveByManager;

public sealed class ApproveByManagerCommandHandler(IHrRequestRepository repository)
    : IRequestHandler<ApproveByManagerCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(ApproveByManagerCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Request '{request.RequestId}' not found.");

        if (entity.ReviewedBy != request.ManagerId)
            throw new InvalidOperationException("This request is not assigned to the specified manager.");

        entity.SetStatus(RequestStatus.Approved, request.ManagerId, request.HrComment);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
