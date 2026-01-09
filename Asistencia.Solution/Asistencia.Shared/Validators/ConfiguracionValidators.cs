using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateConfiguracionRequestValidator : AbstractValidator<CreateConfiguracionRequest>
{
    public CreateConfiguracionRequestValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .GreaterThan(0).WithMessage("El ID de la empresa es inválido");
        RuleFor(x => x.Clave)
            .NotEmpty().WithMessage("La clave es requerida")
            .MaximumLength(100).WithMessage("La clave no puede exceder 100 caracteres");
        RuleFor(x => x.Valor)
            .NotEmpty().WithMessage("El valor es requerido");
    }
}

public class UpdateConfiguracionRequestValidator : AbstractValidator<UpdateConfiguracionRequest>
{
    public UpdateConfiguracionRequestValidator()
    {
        RuleFor(x => x.IdConfiguracion)
            .GreaterThan(0).WithMessage("El ID de la configuración es inválido");
        RuleFor(x => x.IdEmpresa)
            .GreaterThan(0).WithMessage("El ID de la empresa es inválido");
        RuleFor(x => x.Clave)
            .NotEmpty().WithMessage("La clave es requerida")
            .MaximumLength(100).WithMessage("La clave no puede exceder 100 caracteres");
        RuleFor(x => x.Valor)
            .NotEmpty().WithMessage("El valor es requerido");
    }
}
