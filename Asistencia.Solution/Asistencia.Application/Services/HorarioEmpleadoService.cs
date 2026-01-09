using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class HorarioEmpleadoService : IHorarioEmpleadoService
{
    private readonly IHorarioEmpleadoRepository _repository;
    private readonly IValidator<CreateHorarioEmpleadoRequest> _createValidator;
    private readonly IValidator<UpdateHorarioEmpleadoRequest> _updateValidator;

    public HorarioEmpleadoService(
        IHorarioEmpleadoRepository repository,
        IValidator<CreateHorarioEmpleadoRequest> createValidator,
        IValidator<UpdateHorarioEmpleadoRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<HorarioEmpleadoResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<HorarioEmpleadoResponse>>.Success(result);
    }

    public async Task<Result<HorarioEmpleadoResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<HorarioEmpleadoResponse>.Failure("HorarioEmpleado not found");
        return Result<HorarioEmpleadoResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateHorarioEmpleadoRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateHorarioEmpleadoRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdHorarioEmpleado);
        if (existing == null) return Result.Failure("HorarioEmpleado not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("HorarioEmpleado not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
