using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _repository;
    private readonly IValidator<CreateRolRequest> _createValidator;
    private readonly IValidator<UpdateRolRequest> _updateValidator;

    public RolService(
        IRolRepository repository,
        IValidator<CreateRolRequest> createValidator,
        IValidator<UpdateRolRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<RolResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<RolResponse>>.Success(result);
    }

    public async Task<Result<RolResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<RolResponse>.Failure("Rol not found");
        return Result<RolResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateRolRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateRolRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdRol);
        if (existing == null) return Result.Failure("Rol not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Rol not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
