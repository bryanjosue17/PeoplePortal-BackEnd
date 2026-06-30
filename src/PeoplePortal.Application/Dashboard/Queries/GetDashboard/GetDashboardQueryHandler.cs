using MediatR;
using PeoplePortal.Application.Announcements.Mappings;
using PeoplePortal.Application.Benefits.Mappings;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Dashboard.Dtos;
using PeoplePortal.Application.Documents.Mappings;
using PeoplePortal.Application.Employees.Mappings;
using PeoplePortal.Application.Requests.Mappings;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Dashboard.Queries.GetDashboard;

public sealed class GetDashboardQueryHandler(
    IEmployeeRepository employeeRepository,
    IHrRequestRepository hrRequestRepository,
    IDocumentRepository documentRepository,
    IAnnouncementRepository announcementRepository,
    IBenefitRepository benefitRepository)
    : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByKeycloakIdAsync(request.EmployeeId, cancellationToken);

        var allRequests = await hrRequestRepository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        var recentRequests = allRequests.OrderByDescending(x => x.CreatedAtUtc).Take(5).Select(x => x.ToDto()).ToArray();

        var allDocuments = await documentRepository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        var recentDocuments = allDocuments.OrderByDescending(x => x.UploadedAt).Take(5).Select(x => x.ToDto()).ToArray();

        var activeAnnouncements = (await announcementRepository.GetActiveAsync(cancellationToken))
            .Select(x => x.ToDto()).ToArray();

        var availableBenefits = (await benefitRepository.GetActiveAsync(cancellationToken))
            .Select(x => x.ToDto()).ToArray();

        var pendingCount = allRequests.Count(x => x.Status == RequestStatus.Submitted);

        return new DashboardDto(
            employee?.ToDto(),
            recentRequests,
            recentDocuments,
            activeAnnouncements,
            availableBenefits,
            pendingCount);
    }
}
