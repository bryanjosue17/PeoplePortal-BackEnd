using MediatR;
using PeoplePortal.Application.Common.Interfaces;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Requests.Commands.CreateCertificateRequest;

public sealed class CreateCertificateRequestCommandHandler(IHrRequestRepository repository, IEventBus eventBus)
    : IRequestHandler<CreateCertificateRequestCommand, HrRequestDto>
{
    public async Task<HrRequestDto> Handle(CreateCertificateRequestCommand request, CancellationToken cancellationToken)
    {
        HrRequest entity = HrRequest.CreateCertificate(request.EmployeeId, request.CertificateType, request.Reason);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync("hr.request.submitted", new { entity.Id, entity.EmployeeId, Type = entity.Type.ToString(), entity.CreatedAtUtc }, cancellationToken);

        return entity.ToDto();
    }
}