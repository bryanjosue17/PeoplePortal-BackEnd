using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Application.Documents.Mappings;

namespace PeoplePortal.Application.Documents.Queries.GetMyDocuments;

public sealed class GetMyDocumentsQueryHandler(IDocumentRepository repository)
    : IRequestHandler<GetMyDocumentsQuery, IReadOnlyList<DocumentDto>>
{
    public async Task<IReadOnlyList<DocumentDto>> Handle(GetMyDocumentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return entities.Select(x => x.ToDto()).ToArray();
    }
}
