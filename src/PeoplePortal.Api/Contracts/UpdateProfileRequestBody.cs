using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record UpdateProfileRequestBody(string? Phone, string? EmergencyContact, string? Site);
