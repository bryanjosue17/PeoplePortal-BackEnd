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

        var request = HrRequest.CreateVacation("user-123", null, start, end, "Family trip");

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
        request.HrComment.Should().Be("Approved");
    }

    [Fact]
    public void CreateVacation_WithEndDateBeforeStart_ShouldThrow()
    {
        var start = new DateOnly(2026, 7, 5);
        var end = new DateOnly(2026, 7, 1);

        var act = () => HrRequest.CreateVacation("user-123", null, start, end, "Bad trip");

        act.Should().Throw<ArgumentException>().WithParameterName("endDate");
    }

    [Fact]
    public void CreateVoucher_ShouldSetVoucherTypeAndSubmittedStatus()
    {
        var request = HrRequest.CreateVoucher("user-123", "2026-06", "Grocery");

        request.Type.Should().Be(RequestType.Voucher);
        request.Status.Should().Be(RequestStatus.Submitted);
        request.Period.Should().Be("2026-06");
        request.Reason.Should().Be("Grocery");
    }

    [Fact]
    public void Cancel_ByOwner_ShouldSetCancelledStatus()
    {
        var request = HrRequest.CreateVacation("user-123", null, new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 3), "Trip");

        request.Cancel("user-123");

        request.Status.Should().Be(RequestStatus.Cancelled);
    }

    [Fact]
    public void Cancel_ByNonOwner_ShouldThrow()
    {
        var request = HrRequest.CreateCertificate("user-123", "Constancia", "Bank");

        var act = () => request.Cancel("other-user");

        act.Should().Throw<InvalidOperationException>().WithMessage("Only the owner can cancel this request.");
    }

    [Fact]
    public void Cancel_OnFinalizedRequest_ShouldThrow()
    {
        var request = HrRequest.CreateCertificate("user-123", "Constancia", "Bank");
        request.SetStatus(RequestStatus.Approved, "hr-user", "Approved");

        var act = () => request.Cancel("user-123");

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot cancel a finalized request.");
    }

    [Fact]
    public void SetStatus_OnFinalizedRequest_ShouldThrow()
    {
        var request = HrRequest.CreateVacation("user-123", null, new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 3), "Trip");
        request.SetStatus(RequestStatus.Approved, "hr-user", "Approved");

        var act = () => request.SetStatus(RequestStatus.Rejected, "hr-user", "Changed mind");

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot change status of a finalized request.");
    }
}