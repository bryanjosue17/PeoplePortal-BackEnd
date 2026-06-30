using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Application.Documents.Mappings;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Documents.Commands.UploadDocument;

public sealed class UploadDocumentCommandHandler(IDocumentRepository repository)
    : IRequestHandler<UploadDocumentCommand, DocumentDto>
{
    public async Task<DocumentDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        Document entity = Document.Create(request.EmployeeId, request.Name, request.Type, request.ExpiresAt);
        entity.Upload(request.FileUrl);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
