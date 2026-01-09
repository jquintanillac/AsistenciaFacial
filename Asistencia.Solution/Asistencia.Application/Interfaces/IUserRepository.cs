using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IUserRepository
{
    Task<UsuarioResponse?> GetByUsernameAsync(string username);
    Task<UsuarioResponse?> GetByEmailAsync(string email);
    Task<string?> GetPasswordHashAsync(string username);
    Task<IEnumerable<UsuarioResponse>> GetAllAsync();
    Task<UsuarioResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateUsuarioRequest request);
    Task UpdateAsync(UpdateUsuarioRequest request);
    Task DeleteAsync(int id);
}
