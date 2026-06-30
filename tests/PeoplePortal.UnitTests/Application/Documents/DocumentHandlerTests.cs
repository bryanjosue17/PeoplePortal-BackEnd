using FluentAssertions;
using NSubstitute;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Documents.Commands.UploadDocument;
using PeoplePortal.Application.Documents.Commands.UpdateDocumentStatus;
using PeoplePortal.Domain.Entities;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.UnitTests.Application.Documents;

public class DocumentHandlerTests
{
    [Fact]
    public async Task UploadDocumentHandler_ShouldPersistAndReturnDto()
    {
        var repository = Substitute.For<IDocumentRepository>();
        var handler = new UploadDocumentCommandHandler(repository);
        var command = new UploadDocumentCommand(
            "emp-1",
            "Passport",
            "ID",
            "https://storage/files/passport.pdf",
            null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.EmployeeId.Should().Be("emp-1");
        result.Name.Should().Be("Passport");
        result.Type.Should().Be("ID");
        result.Status.Should().Be(DocumentStatus.Available.ToString());
        result.FileUrl.Should().Be("https://storage/files/passport.pdf");

        await repository.Received(1).AddAsync(
            Arg.Is<Document>(x =>
                x.EmployeeId == "emp-1" &&
                x.Name == "Passport" &&
                x.Status == DocumentStatus.Available),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDocumentStatusHandler_ShouldUpdateAndReturnDto()
    {
        var repository = Substitute.For<IDocumentRepository>();
        var existing = Document.Create("emp-1", "Passport", "ID");
        existing.Upload("https://storage/files/passport.pdf");

        repository
            .GetByIdAsync(existing.Id, Arg.Any<CancellationToken>())
            .Returns(existing);

        var handler = new UpdateDocumentStatusCommandHandler(repository);
        var command = new UpdateDocumentStatusCommand(existing.Id, "Approved", "reviewer-1");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Status.Should().Be(DocumentStatus.Approved.ToString());
        result.ReviewedBy.Should().Be("reviewer-1");
        existing.Status.Should().Be(DocumentStatus.Approved);

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDocumentStatusHandler_WhenDocumentNotFound_ShouldThrow()
    {
        var repository = Substitute.For<IDocumentRepository>();
        var docId = Guid.NewGuid();

        repository
            .GetByIdAsync(docId, Arg.Any<CancellationToken>())
            .Returns((Document?)null);

        var handler = new UpdateDocumentStatusCommandHandler(repository);
        var command = new UpdateDocumentStatusCommand(docId, "Approved", "reviewer-1");

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        await repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDocumentStatusHandler_WithInvalidStatus_ShouldThrow()
    {
        var repository = Substitute.For<IDocumentRepository>();
        var existing = Document.Create("emp-1", "Passport", "ID");

        repository
            .GetByIdAsync(existing.Id, Arg.Any<CancellationToken>())
            .Returns(existing);

        var handler = new UpdateDocumentStatusCommandHandler(repository);
        var command = new UpdateDocumentStatusCommand(existing.Id, "InvalidStatus", null);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
        await repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
