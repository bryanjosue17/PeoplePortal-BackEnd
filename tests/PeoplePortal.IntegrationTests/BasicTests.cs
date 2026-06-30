using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.IntegrationTests;

public class BasicTests
{
    [Fact]
    public void Benefit_Create_ShouldSetProperties()
    {
        var benefit = Benefit.Create("Seguro médico", "Salud", "Cobertura completa");
        benefit.Name.Should().Be("Seguro médico");
        benefit.Type.Should().Be("Salud");
        benefit.Description.Should().Be("Cobertura completa");
        benefit.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Employee_Create_ShouldSetAllFields()
    {
        var employee = Employee.Create(
            "keycloak-123", "EMP001", "Juan Pérez", "juan@test.com",
            "TI", "Developer", new DateOnly(2024, 1, 15),
            ContractType.Permanent);

        employee.FullName.Should().Be("Juan Pérez");
        employee.Email.Should().Be("juan@test.com");
        employee.Status.Should().Be(EmployeeStatus.Active);
        employee.HireDate.Should().Be(new DateOnly(2024, 1, 15));
    }

    [Fact]
    public void HrRequest_CreateVacation_ShouldHaveCorrectType()
    {
        var request = HrRequest.CreateVacation(
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            "Vacaciones anuales");

        request.Type.Should().Be(RequestType.Vacation);
        request.Status.Should().Be(RequestStatus.Submitted);
    }

    [Fact]
    public void DocumentStatus_Values_ShouldBeCorrect()
    {
        ((int)DocumentStatus.Available).Should().Be(1);
        ((int)DocumentStatus.Pending).Should().Be(2);
        ((int)DocumentStatus.Expired).Should().Be(6);
    }

    [Fact]
    public void EmployeeStatus_Values_ShouldBeCorrect()
    {
        ((int)EmployeeStatus.Active).Should().Be(1);
        ((int)EmployeeStatus.OnLeave).Should().Be(3);
        ((int)EmployeeStatus.Terminated).Should().Be(4);
    }
}
