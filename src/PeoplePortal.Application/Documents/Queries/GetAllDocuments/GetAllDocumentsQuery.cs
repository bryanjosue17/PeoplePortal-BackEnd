using MediatR;
using PeoplePortal.Application.Documents.Dtos;

namespace PeoplePortal.Application.Documents.Queries.GetAllDocuments;

public sealed record GetAllDocumentsQuery() : IRequest<IReadOnlyList<DocumentDto>>;
