using FluentValidation;
using PeoplePortal.Application.Requests.Commands.CreateVacationRequest;

namespace PeoplePortal.Application.Requests.Validators;

public sealed class CreateVacationRequestValidator : AbstractValidator<CreateVacationRequestCommand>
{
    public CreateVacationRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater or equal than start date.");
    }
}
