using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateHorarioEmpleadoRequestValidator : AbstractValidator<CreateHorarioEmpleadoRequest>
{
    public CreateHorarioEmpleadoRequestValidator()
    {
        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0).WithMessage("El ID del empleado es inválido");
        RuleFor(x => x.IdHorario)
            .GreaterThan(0).WithMessage("El ID del horario es inválido");
    }
}

public class UpdateHorarioEmpleadoRequestValidator : AbstractValidator<UpdateHorarioEmpleadoRequest>
{
    public UpdateHorarioEmpleadoRequestValidator()
    {
        RuleFor(x => x.IdHorarioEmpleado)
            .GreaterThan(0).WithMessage("El ID del horario empleado es inválido");
        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0).WithMessage("El ID del empleado es inválido");
        RuleFor(x => x.IdHorario)
            .GreaterThan(0).WithMessage("El ID del horario es inválido");
    }
}
