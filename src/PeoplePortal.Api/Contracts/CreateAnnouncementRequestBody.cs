using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateAnnouncementRequestBody(
    [property: Required] string Title,
    [property: Required] string Body,
    [property: Required] string Type,
    DateOnly? ExpiresAt);
