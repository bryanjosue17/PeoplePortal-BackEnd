using PeoplePortal.Application.Announcements.Dtos;
using PeoplePortal.Application.Benefits.Dtos;
using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Application.Employees.Dtos;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Dashboard.Dtos;

public sealed record DashboardDto(
    EmployeeDto? Employee,
    IReadOnlyList<HrRequestDto> RecentRequests,
    IReadOnlyList<DocumentDto> RecentDocuments,
    IReadOnlyList<AnnouncementDto> ActiveAnnouncements,
    IReadOnlyList<BenefitDto> AvailableBenefits,
    int PendingRequestsCount);
