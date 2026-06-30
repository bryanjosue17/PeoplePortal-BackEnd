using MediatR;
using PeoplePortal.Application.Documents.Dtos;

namespace PeoplePortal.Application.Documents.Commands.UploadDocument;

public sealed record UploadDocumentCommand(
    string EmployeeId,
    string Name,
    string Type,
    string FileUrl,
    DateOnly? ExpiresAt) : IRequest<DocumentDto>;
