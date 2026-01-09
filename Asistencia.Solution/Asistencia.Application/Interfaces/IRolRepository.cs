using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IRolRepository
{
    Task<IEnumerable<RolResponse>> GetAllAsync();
    Task<RolResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateRolRequest request);
    Task UpdateAsync(UpdateRolRequest request);
    Task DeleteAsync(int id);
}
