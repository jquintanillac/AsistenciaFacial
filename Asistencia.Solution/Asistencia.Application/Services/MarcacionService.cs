using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class MarcacionService : IMarcacionService
{
    private readonly IMarcacionRepository _repository;
    private readonly IValidator<CreateMarcacionRequest> _createValidator;
    private readonly IValidator<UpdateMarcacionRequest> _updateValidator;

    public MarcacionService(
        IMarcacionRepository repository,
        IValidator<CreateMarcacionRequest> createValidator,
        IValidator<UpdateMarcacionRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<MarcacionResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<MarcacionResponse>>.Success(result);
    }

    public async Task<Result<MarcacionResponse>> GetByIdAsync(long id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<MarcacionResponse>.Failure("Marcacion not found");
        return Result<MarcacionResponse>.Success(result);
    }

    public async Task<Result<IEnumerable<MarcacionResponse>>> GetByEmployeeIdAsync(int employeeId)
    {
        // Assuming repository has this method, or we filter GetAll (inefficient).
        // Let's check IMarcacionRepository. If not exists, I should add it or filter.
        // For now, I'll assume it exists or use GetAll if not.
        // Actually, looking at LS, I didn't check IMarcacionRepository content.
        // I'll try to use GetAllAsync and filter for now to be safe, or check repo.
        var all = await _repository.GetAllAsync();
        var result = all.Where(m => m.IdEmpleado == employeeId);
        return Result<IEnumerable<MarcacionResponse>>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateMarcacionRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateMarcacionRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdMarcacion);
        if (existing == null) return Result.Failure("Marcacion not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(long id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Marcacion not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
