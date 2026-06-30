namespace PeoplePortal.Api.Contracts;

public sealed record CreateCertificateRequestBody(
    string CertificateType,
    string? Reason);