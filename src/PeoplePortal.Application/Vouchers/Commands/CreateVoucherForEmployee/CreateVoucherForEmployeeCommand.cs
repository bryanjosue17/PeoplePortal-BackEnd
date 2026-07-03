using MediatR;
using PeoplePortal.Application.Vouchers.Dtos;
using PeoplePortal.Domain.Enums;

namespace PeoplePortal.Application.Vouchers.Commands.CreateVoucherForEmployee;

public sealed record CreateVoucherForEmployeeCommand(
    string EmployeeId,
    string Period,
    NominaType NominaType,
    string? Notes) : IRequest<VoucherDto>;
