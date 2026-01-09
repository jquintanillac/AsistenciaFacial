using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IUsuarioService
{
    Task<Result<IEnumerable<UsuarioResponse>>> GetAllAsync();
    Task<Result<UsuarioResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateUsuarioRequest request);
    Task<Result> UpdateAsync(UpdateUsuarioRequest request);
    Task<Result> DeleteAsync(int id);
}
