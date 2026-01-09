using Asistencia.Application.Common;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class RolUsuarioService : IRolUsuarioService
{
    private readonly IRolUsuarioRepository _repository;
    private readonly IValidator<CreateRolUsuarioRequest> _createValidator;

    public RolUsuarioService(
        IRolUsuarioRepository repository,
        IValidator<CreateRolUsuarioRequest> createValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
    }

    public async Task<Result<IEnumerable<RolUsuarioResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<RolUsuarioResponse>>.Success(result);
    }

    public async Task<Result<IEnumerable<RolUsuarioResponse>>> GetByUserIdAsync(int idUsuario)
    {
        var result = await _repository.GetByUserIdAsync(idUsuario);
        return Result<IEnumerable<RolUsuarioResponse>>.Success(result);
    }

    public async Task<Result> AssignRoleAsync(CreateRolUsuarioRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // Check if assignment already exists? 
        // For now, just try to add. The DB might throw if duplicate, or repository handles it.
        // Usually good to check, but repository interface doesn't have "GetByUserIdAndRoleId".
        // We'll rely on DB constraints or repository logic.
        
        await _repository.AddAsync(request);
        return Result.Success();
    }

    public async Task<Result> RemoveRoleAsync(int idUsuario, int idRol)
    {
        // Ideally check if exists, but repository GetByUserIdAsync returns list.
        // We can just call delete.
        await _repository.DeleteAsync(idUsuario, idRol);
        return Result.Success();
    }
}
