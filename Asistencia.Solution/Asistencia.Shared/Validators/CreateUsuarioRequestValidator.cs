using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class CreateUsuarioRequestValidator : AbstractValidator<CreateUsuarioRequest>
{
    public CreateUsuarioRequestValidator()
    {
        // RuleFor(x => x.IdEmpleado).GreaterThan(0); // Allow 0 for non-employee users (like pure Admins)
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
