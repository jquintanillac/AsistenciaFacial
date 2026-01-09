using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IHorarioRepository
{
    Task<IEnumerable<HorarioResponse>> GetAllAsync();
    Task<HorarioResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateHorarioRequest request);
    Task UpdateAsync(UpdateHorarioRequest request);
    Task DeleteAsync(int id);
}
