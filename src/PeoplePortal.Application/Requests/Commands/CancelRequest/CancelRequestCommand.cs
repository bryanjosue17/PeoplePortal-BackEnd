using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Commands.CancelRequest;

public sealed record CancelRequestCommand(
    Guid RequestId,
    string EmployeeId) : IRequest<HrRequestDto>;
