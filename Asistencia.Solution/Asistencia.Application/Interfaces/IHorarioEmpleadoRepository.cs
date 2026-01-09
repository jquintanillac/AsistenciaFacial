using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IHorarioEmpleadoRepository
{
    Task<IEnumerable<HorarioEmpleadoResponse>> GetAllAsync();
    Task<HorarioEmpleadoResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateHorarioEmpleadoRequest request);
    Task UpdateAsync(UpdateHorarioEmpleadoRequest request);
    Task DeleteAsync(int id);
}
