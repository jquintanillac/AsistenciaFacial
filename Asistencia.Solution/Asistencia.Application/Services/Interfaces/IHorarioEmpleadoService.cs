using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IHorarioEmpleadoService
{
    Task<Result<IEnumerable<HorarioEmpleadoResponse>>> GetAllAsync();
    Task<Result<HorarioEmpleadoResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateHorarioEmpleadoRequest request);
    Task<Result> UpdateAsync(UpdateHorarioEmpleadoRequest request);
    Task<Result> DeleteAsync(int id);
}
