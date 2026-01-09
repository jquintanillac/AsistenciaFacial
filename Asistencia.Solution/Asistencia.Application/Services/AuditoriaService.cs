using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repository;
    private readonly IValidator<CreateAuditoriaRequest> _createValidator;
    private readonly IValidator<UpdateAuditoriaRequest> _updateValidator;

    public AuditoriaService(
        IAuditoriaRepository repository,
        IValidator<CreateAuditoriaRequest> createValidator,
        IValidator<UpdateAuditoriaRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<AuditoriaResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<AuditoriaResponse>>.Success(result);
    }

    public async Task<Result<AuditoriaResponse>> GetByIdAsync(long id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<AuditoriaResponse>.Failure("Auditoria not found");
        return Result<AuditoriaResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateAuditoriaRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateAuditoriaRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdAuditoria);
        if (existing == null) return Result.Failure("Auditoria not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(long id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Auditoria not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
