using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IRolUsuarioRepository
{
    Task<IEnumerable<RolUsuarioResponse>> GetAllAsync();
    Task<IEnumerable<RolUsuarioResponse>> GetByUserIdAsync(int idUsuario);
    Task AddAsync(CreateRolUsuarioRequest request);
    Task DeleteAsync(int idUsuario, int idRol);
}
