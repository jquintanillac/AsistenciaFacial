using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IRolUsuarioService
{
    Task<Result<IEnumerable<RolUsuarioResponse>>> GetAllAsync();
    Task<Result<IEnumerable<RolUsuarioResponse>>> GetByUserIdAsync(int idUsuario);
    Task<Result> AssignRoleAsync(CreateRolUsuarioRequest request);
    Task<Result> RemoveRoleAsync(int idUsuario, int idRol);
}
