using MediatR;
using PeoplePortal.Application.Requests.Dtos;

namespace PeoplePortal.Application.Requests.Commands.CreateVacationRequest;

public sealed record CreateVacationRequestCommand(
    string EmployeeId,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Reason) : IRequest<HrRequestDto>;