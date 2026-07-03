using MediatR;
using PeoplePortal.Application.Vouchers.Dtos;

namespace PeoplePortal.Application.Vouchers.Commands.CreateVoucherForEmployee;

public sealed record CreateVoucherForEmployeeCommand(
    string EmployeeId,
    string Period,
    string? Reason) : IRequest<VoucherDto>;
