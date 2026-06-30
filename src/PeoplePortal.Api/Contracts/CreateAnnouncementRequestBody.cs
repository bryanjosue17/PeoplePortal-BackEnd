namespace PeoplePortal.Api.Contracts;

public sealed record CreateAnnouncementRequestBody(
    string Title,
    string Body,
    string Type,
    DateOnly? ExpiresAt);
