using System.ComponentModel.DataAnnotations;

namespace PeoplePortal.Api.Contracts;

public sealed record CreateCertificateRequestBody(
    [property: Required, StringLength(100)] string CertificateType,
    string? Reason);