using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Employees.Mappings;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(IEmployeeRepository repository)
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ContractType>(request.ContractType, ignoreCase: true, out var contractType))
            throw new ArgumentException($"Invalid ContractType: {request.ContractType}");

        var existing = await repository.GetByKeycloakIdAsync(request.KeycloakId, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"An employee with KeycloakId '{request.KeycloakId}' already exists.");

        Employee entity = Employee.Create(
            request.KeycloakId, request.Code, request.FullName, request.Email,
            request.Department, request.Position, request.HireDate,
            contractType, request.Phone, request.EmergencyContact,
            request.Site, request.ManagerId);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
