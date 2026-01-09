using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class EnrolamientoService : IEnrolamientoService
{
    private readonly IEnrolamientoRepository _repository;
    private readonly IValidator<CreateEnrolamientoRequest> _createValidator;
    private readonly IValidator<UpdateEnrolamientoRequest> _updateValidator;

    public EnrolamientoService(
        IEnrolamientoRepository repository,
        IValidator<CreateEnrolamientoRequest> createValidator,
        IValidator<UpdateEnrolamientoRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<EnrolamientoResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<EnrolamientoResponse>>.Success(result);
    }

    public async Task<Result<EnrolamientoResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<EnrolamientoResponse>.Failure("Enrolamiento not found");
        return Result<EnrolamientoResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateEnrolamientoRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateEnrolamientoRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdEnrolamiento);
        if (existing == null) return Result.Failure("Enrolamiento not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Enrolamiento not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
