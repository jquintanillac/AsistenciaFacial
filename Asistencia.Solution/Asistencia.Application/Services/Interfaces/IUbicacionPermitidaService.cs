using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IUbicacionPermitidaService
{
    Task<Result<IEnumerable<UbicacionPermitidaResponse>>> GetAllAsync();
    Task<Result<UbicacionPermitidaResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateUbicacionPermitidaRequest request);
    Task<Result> UpdateAsync(UpdateUbicacionPermitidaRequest request);
    Task<Result> DeleteAsync(int id);
}
