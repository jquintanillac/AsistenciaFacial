using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IRolService
{
    Task<Result<IEnumerable<RolResponse>>> GetAllAsync();
    Task<Result<RolResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateRolRequest request);
    Task<Result> UpdateAsync(UpdateRolRequest request);
    Task<Result> DeleteAsync(int id);
}
