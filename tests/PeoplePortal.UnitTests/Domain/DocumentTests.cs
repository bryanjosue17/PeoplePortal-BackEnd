using FluentAssertions;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Domain;

public class DocumentTests
{
    [Fact]
    public void Create_ShouldSetPendingStatus()
    {
        var doc = Document.Create("emp-1", "Passport", "ID");

        doc.EmployeeId.Should().Be("emp-1");
        doc.Name.Should().Be("Passport");
        doc.Type.Should().Be("ID");
        doc.Status.Should().Be(DocumentStatus.Pending);
        doc.FileUrl.Should().BeNull();
        doc.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Upload_ShouldSetAvailableAndFileUrl()
    {
        var doc = Document.Create("emp-1", "Passport", "ID");

        doc.Upload("https://storage/files/passport.pdf");

        doc.Status.Should().Be(DocumentStatus.Available);
        doc.FileUrl.Should().Be("https://storage/files/passport.pdf");
    }

    [Fact]
    public void SetStatus_ShouldUpdateStatus()
    {
        var doc = Document.Create("emp-1", "Passport", "ID");

        doc.SetStatus(DocumentStatus.InReview, "reviewer-1");

        doc.Status.Should().Be(DocumentStatus.InReview);
        doc.ReviewedBy.Should().Be("reviewer-1");
    }

    [Fact]
    public void Create_WithoutEmployeeId_ShouldThrow()
    {
        var act = () => Document.Create("", "Passport", "ID");

        act.Should().Throw<ArgumentException>().WithParameterName("employeeId");
    }

    [Fact]
    public void SetStatus_WithAvailable_ShouldThrow()
    {
        var doc = Document.Create("emp-1", "Passport", "ID");

        var act = () => doc.SetStatus(DocumentStatus.Available);

        act.Should().Throw<ArgumentException>().WithParameterName("status");
    }
}
