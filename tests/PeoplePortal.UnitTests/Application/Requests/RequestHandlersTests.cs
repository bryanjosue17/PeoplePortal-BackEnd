using FluentAssertions;
using NSubstitute;
using PeoplePortal.Application.Common.Interfaces;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Requests.Commands.CreateCertificateRequest;
using PeoplePortal.Application.Requests.Commands.CreateVacationRequest;
using PeoplePortal.Application.Requests.Commands.UpdateRequestStatus;
using PeoplePortal.Application.Requests.Queries.GetMyRequests;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Application.Requests;

public class RequestHandlersTests
{
    [Fact]
    public async Task CreateVacationHandler_ShouldPersistAndReturnDto()
    {
        var repository = Substitute.For<IHrRequestRepository>();
        var eventBus = Substitute.For<IEventBus>();
        var handler = new CreateVacationRequestCommandHandler(repository, eventBus);
        var command = new CreateVacationRequestCommand(
            "employee-1",
            null,
            new DateOnly(2026, 7, 1),
            new DateOnly(2026, 7, 5),
            "Family trip");

        var result = await handler.Handle(command, CancellationToken.None);

        result.EmployeeId.Should().Be("employee-1");
        result.Type.Should().Be(RequestType.Vacation.ToString());
        result.Status.Should().Be(RequestStatus.Submitted.ToString());

        await repository.Received(1).AddAsync(
            Arg.Is<HrRequest>(x =>
                x.EmployeeId == "employee-1" &&
                x.Type == RequestType.Vacation &&
                x.Status == RequestStatus.Submitted),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await eventBus.Received(1).PublishAsync(
            "hr.request.submitted",
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateCertificateHandler_ShouldPersistAndReturnDto()
    {
        var repository = Substitute.For<IHrRequestRepository>();
        var eventBus = Substitute.For<IEventBus>();
        var handler = new CreateCertificateRequestCommandHandler(repository, eventBus);
        var command = new CreateCertificateRequestCommand(
            "employee-2",
            "Constancia laboral",
            "Banco");

        var result = await handler.Handle(command, CancellationToken.None);

        result.EmployeeId.Should().Be("employee-2");
        result.Type.Should().Be(RequestType.Certificate.ToString());
        result.Status.Should().Be(RequestStatus.Submitted.ToString());
        result.CertificateType.Should().Be("Constancia laboral");

        await repository.Received(1).AddAsync(
            Arg.Is<HrRequest>(x =>
                x.EmployeeId == "employee-2" &&
                x.Type == RequestType.Certificate &&
                x.CertificateType == "Constancia laboral"),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await eventBus.Received(1).PublishAsync(
            "hr.request.submitted",
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRequestStatusHandler_ShouldUpdateEntityAndPersist()
    {
        var repository = Substitute.For<IHrRequestRepository>();
        var eventBus = Substitute.For<IEventBus>();
        var existingRequest = HrRequest.CreateCertificate("employee-3", "Carta salarial", "Banco");

        repository
            .GetByIdAsync(existingRequest.Id, Arg.Any<CancellationToken>())
            .Returns(existingRequest);

        var handler = new UpdateRequestStatusCommandHandler(repository, eventBus);
        var command = new UpdateRequestStatusCommand(
            existingRequest.Id,
            RequestStatus.Approved,
            "hr-user",
            "Approved");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Status.Should().Be(RequestStatus.Approved.ToString());
        result.ReviewedBy.Should().Be("hr-user");
        existingRequest.Status.Should().Be(RequestStatus.Approved);

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await eventBus.Received(1).PublishAsync(
            "hr.request.approved",
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRequestStatusHandler_WhenRequestNotFound_ShouldThrow()
    {
        var repository = Substitute.For<IHrRequestRepository>();
        var eventBus = Substitute.For<IEventBus>();
        var requestId = Guid.NewGuid();

        repository
            .GetByIdAsync(requestId, Arg.Any<CancellationToken>())
            .Returns((HrRequest?)null);

        var handler = new UpdateRequestStatusCommandHandler(repository, eventBus);
        var command = new UpdateRequestStatusCommand(
            requestId,
            RequestStatus.Approved,
            "hr-user",
            "Approved");

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        await repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await eventBus.DidNotReceive().PublishAsync(
            Arg.Any<string>(),
            Arg.Any<object>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetMyRequestsHandler_ShouldReturnMappedDtos()
    {
        var repository = Substitute.For<IHrRequestRepository>();
        var employeeId = "employee-4";
        var requests = new List<HrRequest>
        {
            HrRequest.CreateVacation(employeeId, null, new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 3), "Trip"),
            HrRequest.CreateCertificate(employeeId, "Constancia laboral", "Bank")
        };

        repository
            .GetByEmployeeIdAsync(employeeId, Arg.Any<CancellationToken>())
            .Returns(requests);

        var handler = new GetMyRequestsQueryHandler(repository);
        var query = new GetMyRequestsQuery(employeeId);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(x => x.EmployeeId == employeeId);
        result.Select(x => x.Type).Should().Contain(new[]
        {
            RequestType.Vacation.ToString(),
            RequestType.Certificate.ToString()
        });
    }
}
