using FluentValidation;
using Asistencia.Shared.DTOs.Requests;

namespace Asistencia.Shared.Validators;

public class CreateUbicacionPermitidaRequestValidator : AbstractValidator<CreateUbicacionPermitidaRequest>
{
    public CreateUbicacionPermitidaRequestValidator()
    {
        RuleFor(x => x.IdEmpresa)
            .GreaterThan(0).WithMessage("El ID de la empresa es inválido");
        RuleFor(x => x.Latitud)
            .InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90");
        RuleFor(x => x.Longitud)
            .InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180");
        RuleFor(x => x.RadioMetros)
            .GreaterThan(0).WithMessage("El radio permitido debe ser mayor a 0");
    }
}

public class UpdateUbicacionPermitidaRequestValidator : AbstractValidator<UpdateUbicacionPermitidaRequest>
{
    public UpdateUbicacionPermitidaRequestValidator()
    {
        RuleFor(x => x.IdUbicacion)
            .GreaterThan(0).WithMessage("El ID de la ubicación permitida es inválido");
        RuleFor(x => x.IdEmpresa)
            .GreaterThan(0).WithMessage("El ID de la empresa es inválido");
        RuleFor(x => x.Latitud)
            .InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90");
        RuleFor(x => x.Longitud)
            .InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180");
        RuleFor(x => x.RadioMetros)
            .GreaterThan(0).WithMessage("El radio permitido debe ser mayor a 0");
    }
}
