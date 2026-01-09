using Asistencia.Application.Common;
using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using FluentValidation;

namespace Asistencia.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUserRepository _repository;
    private readonly IValidator<CreateUsuarioRequest> _createValidator;
    private readonly IValidator<UpdateUsuarioRequest> _updateValidator;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioService(
        IUserRepository repository,
        IValidator<CreateUsuarioRequest> createValidator,
        IValidator<UpdateUsuarioRequest> updateValidator,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<IEnumerable<UsuarioResponse>>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return Result<IEnumerable<UsuarioResponse>>.Success(result);
    }

    public async Task<Result<UsuarioResponse>> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return Result<UsuarioResponse>.Failure("Usuario not found");
        return Result<UsuarioResponse>.Success(result);
    }

    public async Task<Result<int>> CreateAsync(CreateUsuarioRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existingUser = await _repository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
            return Result<int>.Failure("Username already exists");
        
        var existingEmail = await _repository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
            return Result<int>.Failure("Email already exists");

        // Hash password before saving
        request.Password = _passwordHasher.Hash(request.Password);

        var id = await _repository.AddAsync(request);
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(UpdateUsuarioRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var existing = await _repository.GetByIdAsync(request.IdUsuario);
        if (existing == null) return Result.Failure("Usuario not found");

        // If password is provided, hash it. If null/empty, keep existing? 
        // The repository UpdateAsync likely overwrites PasswordHash. 
        // We need to check if UpdateUsuarioRequest has Password field handling.
        // Usually, for updates, if Password is null, we don't update it. 
        // But the repository UpdateAsync might expect a hash.
        
        // Let's check UpdateUsuarioRequest definition again.
        // It has public string? Password { get; set; }
        
        if (!string.IsNullOrEmpty(request.Password))
        {
            request.Password = _passwordHasher.Hash(request.Password);
        }
        else
        {
             // If password is not provided, we might need to fetch the existing hash 
             // OR the repository handles it. 
             // Let's assume for now we must provide the hash or the repo handles nulls.
             // Checking UserRepository.UpdateAsync logic would be good. 
             // sp_Usuario_UPDATE takes PasswordHash. 
             // If we pass null/empty to SP, it might overwrite with null/empty which is bad.
             // We should fetch existing hash if not provided.
             var currentHash = await _repository.GetPasswordHashAsync(existing.Username);
             request.Password = currentHash;
        }

        await _repository.UpdateAsync(request);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return Result.Failure("Usuario not found");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }
}
