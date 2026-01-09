using Asistencia.Shared.DTOs.Requests;
using FluentValidation;

namespace Asistencia.Shared.Validators;

public class CreateHorarioRequestValidator : AbstractValidator<CreateHorarioRequest>
{
    public CreateHorarioRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.HoraEntrada).NotEmpty(); // TimeSpan validation is tricky, usually handled by JSON binding
        RuleFor(x => x.HoraSalida).NotEmpty();
    }
}
