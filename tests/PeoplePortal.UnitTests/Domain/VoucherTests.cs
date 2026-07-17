using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Domain;

public class VoucherTests
{
    [Fact]
    public void Create_ShouldSetRequestedStatus()
    {
        var voucher = Voucher.Create("emp-1", "2026-06", NominaType.ComprobanteDepago, "Grocery");

        voucher.EmployeeId.Should().Be("emp-1");
        voucher.Period.Should().Be("2026-06");
        voucher.Status.Should().Be(VoucherStatus.Requested);
        voucher.NominaType.Should().Be(NominaType.ComprobanteDepago);
        voucher.Notes.Should().Be("Grocery");
        voucher.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Upload_ShouldSetAvailableForDownload()
    {
        var voucher = Voucher.Create("emp-1", "2026-06");

        voucher.Upload("https://storage/vouchers/june.pdf");

        voucher.Status.Should().Be(VoucherStatus.AvailableForDownload);
        voucher.FileUrl.Should().Be("https://storage/vouchers/june.pdf");
    }

    [Fact]
    public void SetStatus_OnFinalized_ShouldThrow()
    {
        var voucher = Voucher.Create("emp-1", "2026-06");
        voucher.SetStatus(VoucherStatus.Completed);

        var act = () => voucher.SetStatus(VoucherStatus.InProcess);

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot change status of a finalized nomina record.");
    }
}
