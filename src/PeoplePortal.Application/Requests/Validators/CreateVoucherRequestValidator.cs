using FluentValidation;
using PeoplePortal.Application.Requests.Commands.CreateVoucherRequest;

namespace PeoplePortal.Application.Requests.Validators;

public sealed class CreateVoucherRequestValidator : AbstractValidator<CreateVoucherRequestCommand>
{
    public CreateVoucherRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.Period).NotEmpty().MaximumLength(50);
    }
}
