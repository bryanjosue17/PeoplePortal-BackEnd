using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record UpdateDocumentStatusBody(
    [property: Required, StringLength(20)] string Status,
    string? ReviewedBy);
