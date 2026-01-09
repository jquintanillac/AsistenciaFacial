using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IRegistroAsistenciaService
{
    Task<Result<IEnumerable<RegistroAsistenciaResponse>>> GetAllAsync();
    Task<Result<RegistroAsistenciaResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateRegistroAsistenciaRequest request);
    Task<Result> UpdateAsync(UpdateRegistroAsistenciaRequest request);
    Task<Result> DeleteAsync(int id);
}
