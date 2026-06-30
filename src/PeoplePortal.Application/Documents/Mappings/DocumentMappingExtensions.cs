using PeoplePortal.Application.Documents.Dtos;
using PeoplePortal.Domain.Entities;

namespace PeoplePortal.Application.Documents.Mappings;

public static class DocumentMappingExtensions
{
    public static DocumentDto ToDto(this Document document)
    {
        return new DocumentDto(
            document.Id,
            document.EmployeeId,
            document.Name,
            document.Type,
            document.Status.ToString(),
            document.FileUrl,
            document.ExpiresAt,
            document.UploadedAt,
            document.ReviewedBy);
    }
}
