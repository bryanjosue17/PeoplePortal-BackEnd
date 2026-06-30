using MediatR;
using PeoplePortal.Application.Reports.Dtos;

namespace PeoplePortal.Application.Reports.Queries.GetPendingDocuments;

public sealed record GetPendingDocumentsQuery() : IRequest<IReadOnlyList<PendingDocumentsDto>>;
