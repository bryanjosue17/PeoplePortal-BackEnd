using MediatR;
using PeoplePortal.Application.Common.Interfaces;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Requests.Commands.CreateVacationRequest;

public sealed class CreateVacationRequestCommandHandler(IHrRequestRepository repository, IEventBus eventBus)
    : IRequestHandler<CreateVacationRequestCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(CreateVacationRequestCommand request, CancellationToken cancellationToken)
    {
        HrRequest entity = HrRequest.CreateVacation(request.EmployeeId, request.ManagerId, request.StartDate, request.EndDate, request.Reason);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync("hr.request.submitted", new { entity.Id, entity.EmployeeId, Type = entity.Type.ToString(), entity.CreatedAtUtc }, cancellationToken);

        return entity.ToDto();
    }
}