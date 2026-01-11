using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateEnrolamientoRequestValidator : AbstractValidator<CreateEnrolamientoRequest>
{
    public CreateEnrolamientoRequestValidator()
    {
        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0).WithMessage("El ID del empleado es inválido");
        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo es requerido")
            .MaximumLength(50).WithMessage("El tipo no puede exceder 50 caracteres");
        RuleFor(x => x.IdentificadorBiometrico)
            .NotEmpty()
            .When(x => x.Tipo != "Rostro")
            .WithMessage("El identificador biométrico es requerido para este tipo de enrolamiento");

        RuleFor(x => x.DescriptorFacial)
            .NotEmpty()
            .When(x => x.Tipo == "Rostro")
            .WithMessage("El descriptor facial es requerido para enrolamiento facial");
    }
}

public class UpdateEnrolamientoRequestValidator : AbstractValidator<UpdateEnrolamientoRequest>
{
    public UpdateEnrolamientoRequestValidator()
    {
        RuleFor(x => x.IdEnrolamiento)
            .GreaterThan(0).WithMessage("El ID del enrolamiento es inválido");
        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0).WithMessage("El ID del empleado es inválido");
        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo es requerido")
            .MaximumLength(50).WithMessage("El tipo no puede exceder 50 caracteres");
        RuleFor(x => x.IdentificadorBiometrico)
            .NotEmpty().WithMessage("El identificador biométrico es requerido");
    }
}
