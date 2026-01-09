using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class UpdateRegistroAsistenciaRequestValidator : AbstractValidator<UpdateRegistroAsistenciaRequest>
{
    public UpdateRegistroAsistenciaRequestValidator()
    {
        RuleFor(x => x.IdRegistro).GreaterThan(0);
        RuleFor(x => x.IdEmpleado).GreaterThan(0);
        RuleFor(x => x.Fecha).NotEmpty();
        RuleFor(x => x.EstadoAsistencia).NotEmpty().MaximumLength(20);
    }
}
