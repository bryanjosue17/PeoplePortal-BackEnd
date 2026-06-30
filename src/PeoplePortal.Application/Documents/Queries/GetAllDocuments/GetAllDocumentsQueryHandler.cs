using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Application.Documents.Mappings;

namespace PeoplePortal.Application.Documents.Queries.GetAllDocuments;

public sealed class GetAllDocumentsQueryHandler(IDocumentRepository repository)
    : IRequestHandler<GetAllDocumentsQuery, IReadOnlyList<DocumentDto>>
{
    public async Task<IReadOnlyList<DocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
