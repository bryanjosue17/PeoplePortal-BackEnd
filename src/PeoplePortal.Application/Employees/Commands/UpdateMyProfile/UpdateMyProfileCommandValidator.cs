using FluentValidation;

namespace PeoplePortal.Application.Employees.Commands.UpdateMyProfile;

public sealed class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
