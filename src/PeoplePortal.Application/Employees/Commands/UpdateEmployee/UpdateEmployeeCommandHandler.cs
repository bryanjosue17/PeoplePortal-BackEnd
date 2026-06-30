using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Employees.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler(IEmployeeRepository repository)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee '{request.Id}' not found.");

        if (!Enum.TryParse<ContractType>(request.ContractType, ignoreCase: true, out var contractType))
            throw new ArgumentException($"Invalid ContractType: {request.ContractType}");

        if (!Enum.TryParse<EmployeeStatus>(request.Status, ignoreCase: true, out var status))
            throw new ArgumentException($"Invalid EmployeeStatus: {request.Status}");

        entity.UpdateEmployment(request.Department, request.Position, contractType, status);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
