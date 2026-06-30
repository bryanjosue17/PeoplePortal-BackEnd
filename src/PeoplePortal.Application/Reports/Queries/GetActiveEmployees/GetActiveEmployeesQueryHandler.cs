using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Reports.Dtos;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Reports.Queries.GetActiveEmployees;

public sealed class GetActiveEmployeesQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetActiveEmployeesQuery, ActiveEmployeesDto>
{
    public async Task<ActiveEmployeesDto> Handle(GetActiveEmployeesQuery request, CancellationToken cancellationToken)
    {
        var allEmployees = await repository.GetAllAsync(cancellationToken);
        return new ActiveEmployeesDto(
            allEmployees.Count,
            allEmployees.Count(x => x.Status == EmployeeStatus.Active),
            allEmployees.Count(x => x.Status == EmployeeStatus.OnLeave),
            allEmployees.Count(x => x.Status == EmployeeStatus.Inactive),
            allEmployees.Count(x => x.Status == EmployeeStatus.Terminated));
    }
}
