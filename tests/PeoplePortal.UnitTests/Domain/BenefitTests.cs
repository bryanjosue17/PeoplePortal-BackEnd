using FluentAssertions;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.UnitTests.Domain;

public class BenefitTests
{
    [Fact]
    public void Create_ShouldSetActive()
    {
        var benefit = Benefit.Create("Health Insurance", "Medical", "Basic coverage");

        benefit.Name.Should().Be("Health Insurance");
        benefit.Type.Should().Be("Medical");
        benefit.Description.Should().Be("Basic coverage");
        benefit.IsActive.Should().BeTrue();
        benefit.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Deactivate_ShouldSetInactive()
    {
        var benefit = Benefit.Create("Health Insurance", "Medical");

        benefit.Deactivate();

        benefit.IsActive.Should().BeFalse();
    }
}
