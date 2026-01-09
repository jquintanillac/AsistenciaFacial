using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class UbicacionPermitidaService : IUbicacionPermitidaService
{
    private readonly IUbicacionPermitidaRepository _repository;
    private readonly IValidator<CreateUbicacionPermitidaRequest> _createValidator;
    private readonly IValidator<UpdateUbicacionPermitidaRequest> _updateValidator;

    public UbicacionPermitidaService(
        IUbicacionPermitidaRepository repository,
        IValidator<CreateUbicacionPermitidaRequest> createValidator,
        IValidator<UpdateUbicacionPermitidaRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<UbicacionPermitidaResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<UbicacionPermitidaResponse>>.Success(result);
    }

    public async Task<Result<UbicacionPermitidaResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<UbicacionPermitidaResponse>.Failure("UbicacionPermitida not found");
        return Result<UbicacionPermitidaResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateUbicacionPermitidaRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateUbicacionPermitidaRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdUbicacion);
        if (existing == null) return Result.Failure("UbicacionPermitida not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("UbicacionPermitida not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
