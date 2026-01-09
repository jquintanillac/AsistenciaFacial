using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class UpdateEmpresaRequestValidator : AbstractValidator<UpdateEmpresaRequest>
{
    public UpdateEmpresaRequestValidator()
    {
        RuleFor(x => x.IdEmpresa).GreaterThan(0);
        RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RUC).NotEmpty().Length(11).Matches("^[0-9]*$");
        RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
    }
}
