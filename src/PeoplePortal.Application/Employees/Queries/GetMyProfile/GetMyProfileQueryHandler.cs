using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;

namespace PeoplePortal.Application.Employees.Queries.GetMyProfile;

public sealed class GetMyProfileQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetMyProfileQuery, EmployeeDto?>
{
    public async Task<EmployeeDto?> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByKeycloakIdAsync(request.EmployeeId, cancellationToken);
        return entity?.ToDto();
    }
}
