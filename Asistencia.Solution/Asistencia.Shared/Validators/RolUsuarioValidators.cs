using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateRolUsuarioRequestValidator : AbstractValidator<CreateRolUsuarioRequest>
{
    public CreateRolUsuarioRequestValidator()
    {
        RuleFor(x => x.IdRol)
            .GreaterThan(0).WithMessage("El ID del rol es inválido");
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID del usuario es inválido");
    }
}
