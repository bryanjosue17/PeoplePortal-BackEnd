using FluentValidation;
using PeoplePortal.Application.Requests.Commands.CreateVacationRequest;

namespace PeoplePortal.Application.Requests.Validators;

public sealed class CreateVacationRequestValidator : AbstractValidator<CreateVacationRequestCommand>
{
    public CreateVacationRequestValidator()
    {
        var tomorrow = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        RuleFor(x => x.EmployeeId).NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(tomorrow)
            .WithMessage("La fecha de inicio debe ser al menos el día de mañana.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(tomorrow)
            .WithMessage("La fecha de fin debe ser al menos el día de mañana.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater or equal than start date.");
    }
}
