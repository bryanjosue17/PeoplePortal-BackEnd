using FluentValidation;
using PeoplePortal.Application.Requests.Commands.CreateCertificateRequest;

namespace PeoplePortal.Application.Requests.Validators;

public sealed class CreateCertificateRequestValidator : AbstractValidator<CreateCertificateRequestCommand>
{
    public CreateCertificateRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.CertificateType).NotEmpty().MaximumLength(100);
    }
}
