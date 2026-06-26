using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Domain;

public class HrRequestTests
{
    [Fact]
    public void CreateVacation_ShouldSetVacationTypeAndSubmittedStatus()
    {
        var start = new DateOnly(2026, 7, 1);
        var end = new DateOnly(2026, 7, 5);

        var request = HrRequest.CreateVacation("user-123", start, end, "Family trip");

        request.Type.Should().Be(RequestType.Vacation);
        request.Status.Should().Be(RequestStatus.Submitted);
        request.VacationStartDate.Should().Be(start);
        request.VacationEndDate.Should().Be(end);
    }

    [Fact]
    public void SetStatus_ShouldUpdateStatusToApproved()
    {
        var request = HrRequest.CreateCertificate("user-123", "Constancia laboral", "Banco");

        request.SetStatus(RequestStatus.Approved, "hr-user", "Approved");

        request.Status.Should().Be(RequestStatus.Approved);
        request.ReviewedBy.Should().Be("hr-user");
    }
}