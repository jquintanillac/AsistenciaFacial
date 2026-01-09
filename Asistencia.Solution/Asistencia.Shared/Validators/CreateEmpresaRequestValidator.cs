using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class CreateEmpresaRequestValidator : AbstractValidator<CreateEmpresaRequest>
{
    public CreateEmpresaRequestValidator()
    {
        RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RUC).NotEmpty().Length(11).Matches("^[0-9]*$").WithMessage("RUC must be 11 digits.");
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
    }
}
