using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;

namespace PeoplePortal.Application.Employees.Queries.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeDto>>
{
    public async Task<IReadOnlyList<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
