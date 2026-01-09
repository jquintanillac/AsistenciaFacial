using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class UpdateMarcacionRequestValidator : AbstractValidator<UpdateMarcacionRequest>
{
    public UpdateMarcacionRequestValidator()
    {
        RuleFor(x => x.IdMarcacion).GreaterThan(0);
        RuleFor(x => x.IdEmpleado).GreaterThan(0);
        RuleFor(x => x.FechaHora).NotEmpty();
        RuleFor(x => x.TipoMarcacion).NotEmpty().MaximumLength(20);
    }
}
