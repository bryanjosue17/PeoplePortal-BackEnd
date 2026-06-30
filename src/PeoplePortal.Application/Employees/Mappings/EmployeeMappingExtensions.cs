using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Employees.Mappings;

public static class EmployeeMappingExtensions
{
    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto(
            employee.Id,
            employee.KeycloakId,
            employee.Code,
            employee.FullName,
            employee.Email,
            employee.Phone,
            employee.Department,
            employee.Position,
            employee.HireDate,
            employee.ContractType.ToString(),
            employee.Status.ToString(),
            employee.EmergencyContact,
            employee.Site,
            employee.ManagerId,
            employee.CreatedAtUtc,
            employee.UpdatedAtUtc);
    }

    public static Employee ToEmployeeFromCreate(this CreateEmployeeDto dto)
    {
        return Employee.Create(
            dto.KeycloakId,
            dto.Code,
            dto.FullName,
            dto.Email,
            dto.Department,
            dto.Position,
            dto.HireDate,
            Enum.Parse<ContractType>(dto.ContractType),
            dto.Phone,
            dto.EmergencyContact,
            dto.Site,
            dto.ManagerId);
    }
}
