using MediatR;
using PeoplePortal.Application.Requests.Dtos;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Requests.Commands.UpdateRequestStatus;

public sealed record UpdateRequestStatusCommand(
    Guid RequestId,
    RequestStatus Status,
    string ReviewedBy,
    string? HrComment) : IRequest<HrRequestDto>;