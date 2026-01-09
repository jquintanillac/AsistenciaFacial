using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class UpdateHorarioRequestValidator : AbstractValidator<UpdateHorarioRequest>
{
    public UpdateHorarioRequestValidator()
    {
        RuleFor(x => x.IdHorario).GreaterThan(0);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.HoraEntrada).NotEmpty();
        RuleFor(x => x.HoraSalida).NotEmpty();
    }
}
