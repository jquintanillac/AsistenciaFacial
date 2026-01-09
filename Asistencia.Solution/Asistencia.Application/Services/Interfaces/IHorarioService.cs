using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IHorarioService
{
    Task<Result<IEnumerable<HorarioResponse>>> GetAllAsync();
    Task<Result<HorarioResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateHorarioRequest request);
    Task<Result> UpdateAsync(UpdateHorarioRequest request);
    Task<Result> DeleteAsync(int id);
}
