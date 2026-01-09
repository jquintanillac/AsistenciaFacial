using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateRolRequestValidator : AbstractValidator<CreateRolRequest>
{
    public CreateRolRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");
    }
}

public class UpdateRolRequestValidator : AbstractValidator<UpdateRolRequest>
{
    public UpdateRolRequestValidator()
    {
        RuleFor(x => x.IdRol)
            .GreaterThan(0).WithMessage("El ID del rol es inválido");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");
    }
}
