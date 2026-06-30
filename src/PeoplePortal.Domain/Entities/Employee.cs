using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string KeycloakId { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string Department { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public DateOnly HireDate { get; private set; }
    public ContractType ContractType { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public string? EmergencyContact { get; private set; }
    public string? Site { get; private set; }
    public string? ManagerId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Employee()
    {
    }

    public static Employee Create(
        string keycloakId, string code, string fullName, string email,
        string department, string position, DateOnly hireDate,
        ContractType contractType, string? phone = null,
        string? emergencyContact = null, string? site = null, string? managerId = null)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
            throw new ArgumentException("KeycloakId is required.", nameof(keycloakId));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("FullName is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        return new Employee
        {
            Id = Guid.NewGuid(),
            KeycloakId = keycloakId,
            Code = code,
            FullName = fullName,
            Email = email,
            Phone = phone,
            Department = department,
            Position = position,
            HireDate = hireDate,
            ContractType = contractType,
            Status = EmployeeStatus.Active,
            EmergencyContact = emergencyContact,
            Site = site,
            ManagerId = managerId,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string? phone, string? emergencyContact, string? site)
    {
        Phone = phone;
        EmergencyContact = emergencyContact;
        Site = site;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateEmployment(string department, string position, ContractType contractType, EmployeeStatus status)
    {
        Department = department;
        Position = position;
        ContractType = contractType;
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
