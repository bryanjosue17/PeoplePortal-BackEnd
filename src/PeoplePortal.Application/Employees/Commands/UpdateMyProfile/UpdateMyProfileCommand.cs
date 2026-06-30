using MediatR;
using PeoplePortal.Application.Employees.Dtos;

namespace PeoplePortal.Application.Employees.Commands.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(
    string EmployeeId,
    string? Phone,
    string? EmergencyContact,
    string? Site) : IRequest<EmployeeDto>;
