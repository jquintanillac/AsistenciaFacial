using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _repository;
    private readonly IValidator<CreateEmpresaRequest> _createValidator;
    private readonly IValidator<UpdateEmpresaRequest> _updateValidator;

    public EmpresaService(
        IEmpresaRepository repository,
        IValidator<CreateEmpresaRequest> createValidator,
        IValidator<UpdateEmpresaRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<EmpresaResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<EmpresaResponse>>.Success(result);
    }

    public async Task<Result<EmpresaResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<EmpresaResponse>.Failure("Empresa not found");
        return Result<EmpresaResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateEmpresaRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateEmpresaRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdEmpresa);
        if (existing == null) return Result.Failure("Empresa not found");

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Empresa not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
