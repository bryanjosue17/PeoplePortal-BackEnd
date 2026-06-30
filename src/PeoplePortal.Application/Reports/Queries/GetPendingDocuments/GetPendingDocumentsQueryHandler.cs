using MediatR;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Reports.Dtos;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Reports.Queries.GetPendingDocuments;

public sealed class GetPendingDocumentsQueryHandler(
    IEmployeeRepository employeeRepository,
    IDocumentRepository documentRepository)
    : IRequestHandler<GetPendingDocumentsQuery, IReadOnlyList<PendingDocumentsDto>>
{
    public async Task<IReadOnlyList<PendingDocumentsDto>> Handle(GetPendingDocumentsQuery request, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetAllAsync(cancellationToken);
        var documents = await documentRepository.GetAllAsync(cancellationToken);

        return employees.Select(emp =>
        {
            var empDocs = documents.Where(d => d.EmployeeId.ToString() == emp.Id.ToString()).ToList();
            return new PendingDocumentsDto(
                emp.Id,
                emp.FullName,
                emp.Department,
                empDocs.Count(d => d.Status == DocumentStatus.Pending || d.Status == DocumentStatus.InReview),
                empDocs.Count(d => d.Status == DocumentStatus.Expired));
        }).ToArray();
    }
}
