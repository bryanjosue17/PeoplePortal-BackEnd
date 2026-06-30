using FluentAssertions;
using NSubstitute;
using PeoplePortal.Application.Benefits.Queries.GetActiveBenefits;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.UnitTests.Application.Benefits;

public class BenefitHandlerTests
{
    [Fact]
    public async Task GetActiveBenefitsHandler_ShouldReturnMappedDtos()
    {
        var repository = Substitute.For<IBenefitRepository>();
        var entities = new List<Benefit>
        {
            Benefit.Create("Seguro médico", "Salud", "Cobertura médica para el colaborador y familia."),
            Benefit.Create("Bono anual", "Bonificación", "Bono por desempeño anual.")
        };

        repository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(entities.AsReadOnly());

        var handler = new GetActiveBenefitsQueryHandler(repository);
        var result = await handler.Handle(new GetActiveBenefitsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Seguro médico");
        result[0].Type.Should().Be("Salud");
        result[1].Name.Should().Be("Bono anual");
    }

    [Fact]
    public async Task GetActiveBenefitsHandler_WhenNoBenefits_ShouldReturnEmpty()
    {
        var repository = Substitute.For<IBenefitRepository>();
        repository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Benefit>());

        var handler = new GetActiveBenefitsQueryHandler(repository);
        var result = await handler.Handle(new GetActiveBenefitsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetActiveBenefitsHandler_ShouldMapAllFields()
    {
        var repository = Substitute.For<IBenefitRepository>();
        var benefit = Benefit.Create("Convenio farmacia", "Descuento", "20% en farmacia afiliada.");

        repository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Benefit> { benefit }.AsReadOnly());

        var handler = new GetActiveBenefitsQueryHandler(repository);
        var result = await handler.Handle(new GetActiveBenefitsQuery(), CancellationToken.None);

        var dto = result.Single();
        dto.Name.Should().Be("Convenio farmacia");
        dto.Type.Should().Be("Descuento");
        dto.Description.Should().Be("20% en farmacia afiliada.");
        dto.IsActive.Should().BeTrue();
    }
}
