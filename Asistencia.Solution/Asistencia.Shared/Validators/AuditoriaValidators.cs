using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateAuditoriaRequestValidator : AbstractValidator<CreateAuditoriaRequest>
{
    public CreateAuditoriaRequestValidator()
    {
        RuleFor(x => x.Entidad)
            .NotEmpty().WithMessage("La entidad es requerida")
            .MaximumLength(100).WithMessage("La entidad no puede exceder 100 caracteres");
        RuleFor(x => x.Accion)
            .NotEmpty().WithMessage("La acción es requerida")
            .MaximumLength(50).WithMessage("La acción no puede exceder 50 caracteres");
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID del usuario es inválido");
    }
}

public class UpdateAuditoriaRequestValidator : AbstractValidator<UpdateAuditoriaRequest>
{
    public UpdateAuditoriaRequestValidator()
    {
        RuleFor(x => x.IdAuditoria)
            .GreaterThan(0).WithMessage("El ID de la auditoría es inválido");
        RuleFor(x => x.Entidad)
            .NotEmpty().WithMessage("La entidad es requerida")
            .MaximumLength(100).WithMessage("La entidad no puede exceder 100 caracteres");
        RuleFor(x => x.Accion)
            .NotEmpty().WithMessage("La acción es requerida")
            .MaximumLength(50).WithMessage("La acción no puede exceder 50 caracteres");
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID del usuario es inválido");
    }
}
