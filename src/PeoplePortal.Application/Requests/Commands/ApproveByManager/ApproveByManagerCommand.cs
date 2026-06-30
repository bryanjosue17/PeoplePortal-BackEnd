using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Commands.ApproveByManager;

public sealed record ApproveByManagerCommand(
    Guid RequestId,
    string ManagerId,
    string? HrComment) : IRequest<HrRequestDto>;
