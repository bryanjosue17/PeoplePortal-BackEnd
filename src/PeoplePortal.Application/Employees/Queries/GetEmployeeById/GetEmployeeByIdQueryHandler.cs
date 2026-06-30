using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;

namespace PeoplePortal.Application.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdQueryHandler(IEmployeeRepository repository)
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        return entity?.ToDto();
    }
}
