using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class RegistroAsistenciaService : IRegistroAsistenciaService
{
    private readonly IRegistroAsistenciaRepository _repository;
    private readonly IValidator<CreateRegistroAsistenciaRequest> _createValidator;
    private readonly IValidator<UpdateRegistroAsistenciaRequest> _updateValidator;

    public RegistroAsistenciaService(
        IRegistroAsistenciaRepository repository,
        IValidator<CreateRegistroAsistenciaRequest> createValidator,
        IValidator<UpdateRegistroAsistenciaRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<RegistroAsistenciaResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<RegistroAsistenciaResponse>>.Success(result);
    }

    public async Task<Result<RegistroAsistenciaResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<RegistroAsistenciaResponse>.Failure("RegistroAsistencia not found");
        return Result<RegistroAsistenciaResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateRegistroAsistenciaRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateRegistroAsistenciaRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdRegistro);
        if (existing == null) return Result.Failure("RegistroAsistencia not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("RegistroAsistencia not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
