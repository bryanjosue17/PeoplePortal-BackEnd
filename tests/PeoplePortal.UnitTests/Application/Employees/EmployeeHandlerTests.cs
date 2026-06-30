using FluentAssertions;
using NSubstitute;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Employees.Commands.CreateEmployee;
using PeoplePortal.Application.Employees.Commands.UpdateMyProfile;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Application.Employees;

public class EmployeeHandlerTests
{
    [Fact]
    public async Task CreateEmployeeHandler_ShouldPersistAndReturnDto()
    {
        var repository = Substitute.For<IEmployeeRepository>();
        var handler = new CreateEmployeeCommandHandler(repository);
        var command = new CreateEmployeeCommand(
            "kc-1",
            "EMP001",
            "John Doe",
            "john@test.com",
            "555-0100",
            "Engineering",
            "Developer",
            new DateOnly(2025, 1, 15),
            "Permanent",
            "Jane Doe",
            "Office A",
            "mgr-1");

        var result = await handler.Handle(command, CancellationToken.None);

        result.KeycloakId.Should().Be("kc-1");
        result.Code.Should().Be("EMP001");
        result.FullName.Should().Be("John Doe");
        result.Email.Should().Be("john@test.com");
        result.Department.Should().Be("Engineering");
        result.Position.Should().Be("Developer");
        result.ContractType.Should().Be(ContractType.Permanent.ToString());
        result.Status.Should().Be(EmployeeStatus.Active.ToString());

        await repository.Received(1).AddAsync(
            Arg.Is<Employee>(x =>
                x.KeycloakId == "kc-1" &&
                x.Code == "EMP001" &&
                x.Status == EmployeeStatus.Active),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMyProfileHandler_ShouldUpdateAndReturnDto()
    {
        var repository = Substitute.For<IEmployeeRepository>();
        var existing = Employee.Create(
            "kc-1", "EMP001", "John Doe", "john@test.com",
            "Engineering", "Developer", new DateOnly(2025, 1, 15),
            ContractType.Permanent, "555-0100");

        repository
            .GetByKeycloakIdAsync("kc-1", Arg.Any<CancellationToken>())
            .Returns(existing);

        var handler = new UpdateMyProfileCommandHandler(repository);
        var command = new UpdateMyProfileCommand("kc-1", "555-0200", "Mary Doe", "Office B");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Phone.Should().Be("555-0200");
        result.EmergencyContact.Should().Be("Mary Doe");
        result.Site.Should().Be("Office B");

        existing.Phone.Should().Be("555-0200");
        existing.EmergencyContact.Should().Be("Mary Doe");
        existing.Site.Should().Be("Office B");

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMyProfileHandler_WhenEmployeeNotFound_ShouldThrow()
    {
        var repository = Substitute.For<IEmployeeRepository>();
        repository
            .GetByKeycloakIdAsync("unknown", Arg.Any<CancellationToken>())
            .Returns((Employee?)null);

        var handler = new UpdateMyProfileCommandHandler(repository);
        var command = new UpdateMyProfileCommand("unknown", "555-0200", null, null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        await repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
