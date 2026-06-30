using MediatR;
using PeoplePortal.Application.Documents.Dtos;

namespace PeoplePortal.Application.Documents.Queries.GetMyDocuments;

public sealed record GetMyDocumentsQuery(string EmployeeId) : IRequest<IReadOnlyList<DocumentDto>>;
