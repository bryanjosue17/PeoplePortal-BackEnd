using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Requests.Mappings;

public static class HrRequestMappingExtensions
{
    public static HrRequestDto ToDto(this HrRequest request)
    {
        return new HrRequestDto(
            request.Id,
            request.EmployeeId,
            request.Type.ToString(),
            request.Status.ToString(),
            request.VacationStartDate,
            request.VacationEndDate,
            request.CertificateType,
            request.Period,
            request.Reason,
            request.HrComment,
            request.ReviewedBy,
            request.CreatedAtUtc,
            request.UpdatedAtUtc);
    }
}