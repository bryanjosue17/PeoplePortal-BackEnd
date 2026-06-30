using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;

namespace PeoplePortal.Application.Employees.Commands.UpdateMyProfile;

public sealed class UpdateMyProfileCommandHandler(IEmployeeRepository repository)
    : IRequestHandler<UpdateMyProfileCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByKeycloakIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{request.EmployeeId}' not found.");

        entity.UpdateProfile(request.Phone, request.EmergencyContact, request.Site);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
