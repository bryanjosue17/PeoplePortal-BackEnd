using MediatR;
using PeoplePortal.Application.Documents.Dtos;

namespace PeoplePortal.Application.Documents.Commands.UpdateDocumentStatus;

public sealed record UpdateDocumentStatusCommand(
    Guid Id,
    string Status,
    string? ReviewedBy) : IRequest<DocumentDto>;
