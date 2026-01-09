using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class ConfiguracionService : IConfiguracionService
{
    private readonly IConfiguracionRepository _repository;
    private readonly IValidator<CreateConfiguracionRequest> _createValidator;
    private readonly IValidator<UpdateConfiguracionRequest> _updateValidator;

    public ConfiguracionService(
        IConfiguracionRepository repository,
        IValidator<CreateConfiguracionRequest> createValidator,
        IValidator<UpdateConfiguracionRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<ConfiguracionResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<ConfiguracionResponse>>.Success(result);
    }

    public async Task<Result<ConfiguracionResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<ConfiguracionResponse>.Failure("Configuracion not found");
        return Result<ConfiguracionResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateConfiguracionRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateConfiguracionRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdConfiguracion);
        if (existing == null) return Result.Failure("Configuracion not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Configuracion not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
