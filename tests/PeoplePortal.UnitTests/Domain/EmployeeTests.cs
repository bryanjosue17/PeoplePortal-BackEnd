using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Domain;

public class EmployeeTests
{
    [Fact]
    public void Create_ShouldSetAllProperties()
    {
        var hireDate = new DateOnly(2025, 1, 15);
        var employee = Employee.Create(
            "kc-1", "EMP001", "John Doe", "john@test.com",
            "Engineering", "Developer", hireDate,
            ContractType.Permanent, "555-0100",
            "Jane Doe", "Office A", "mgr-1");

        employee.KeycloakId.Should().Be("kc-1");
        employee.Code.Should().Be("EMP001");
        employee.FullName.Should().Be("John Doe");
        employee.Email.Should().Be("john@test.com");
        employee.Department.Should().Be("Engineering");
        employee.Position.Should().Be("Developer");
        employee.HireDate.Should().Be(hireDate);
        employee.ContractType.Should().Be(ContractType.Permanent);
        employee.Status.Should().Be(EmployeeStatus.Active);
        employee.Phone.Should().Be("555-0100");
        employee.EmergencyContact.Should().Be("Jane Doe");
        employee.Site.Should().Be("Office A");
        employee.ManagerId.Should().Be("mgr-1");
        employee.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_WithoutKeycloakId_ShouldThrow()
    {
        var act = () => Employee.Create(
            "", "EMP001", "John", "john@test.com",
            "Eng", "Dev", new DateOnly(2025, 1, 1),
            ContractType.Permanent);

        act.Should().Throw<ArgumentException>().WithParameterName("keycloakId");
    }

    [Fact]
    public void Create_WithoutCode_ShouldThrow()
    {
        var act = () => Employee.Create(
            "kc-1", "", "John", "john@test.com",
            "Eng", "Dev", new DateOnly(2025, 1, 1),
            ContractType.Permanent);

        act.Should().Throw<ArgumentException>().WithParameterName("code");
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateFields()
    {
        var employee = Employee.Create(
            "kc-1", "EMP001", "John", "john@test.com",
            "Eng", "Dev", new DateOnly(2025, 1, 1),
            ContractType.Permanent);

        employee.UpdateProfile("555-0200", "Mary Doe", "Office B");

        employee.Phone.Should().Be("555-0200");
        employee.EmergencyContact.Should().Be("Mary Doe");
        employee.Site.Should().Be("Office B");
        employee.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateEmployment_ShouldUpdateFields()
    {
        var employee = Employee.Create(
            "kc-1", "EMP001", "John", "john@test.com",
            "Eng", "Dev", new DateOnly(2025, 1, 1),
            ContractType.Permanent);

        employee.UpdateEmployment("HR", "Manager", ContractType.Temporary, EmployeeStatus.OnLeave);

        employee.Department.Should().Be("HR");
        employee.Position.Should().Be("Manager");
        employee.ContractType.Should().Be(ContractType.Temporary);
        employee.Status.Should().Be(EmployeeStatus.OnLeave);
        employee.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
