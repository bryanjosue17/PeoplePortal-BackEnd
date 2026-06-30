using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Commands.CreateVoucherRequest;

public sealed record CreateVoucherRequestCommand(
    string EmployeeId,
    string Period,
    string? Reason) : IRequest<HrRequestDto>;
