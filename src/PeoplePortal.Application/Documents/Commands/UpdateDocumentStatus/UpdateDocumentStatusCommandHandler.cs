using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Application.Documents.Mappings;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Documents.Commands.UpdateDocumentStatus;

public sealed class UpdateDocumentStatusCommandHandler(IDocumentRepository repository)
    : IRequestHandler<UpdateDocumentStatusCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(UpdateDocumentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Document '{request.Id}' not found.");

        if (!Enum.TryParse<DocumentStatus>(request.Status, ignoreCase: true, out var status))
            throw new ArgumentException($"Invalid DocumentStatus: {request.Status}");

        entity.SetStatus(status, request.ReviewedBy);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
