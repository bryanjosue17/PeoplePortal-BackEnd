using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Commands.CreateCertificateRequest;

public sealed record CreateCertificateRequestCommand(
    string EmployeeId,
    string CertificateType,
    string? Reason) : IRequest<HrRequestDto>;