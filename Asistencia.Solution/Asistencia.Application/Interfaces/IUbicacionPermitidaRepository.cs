using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IUbicacionPermitidaRepository
{
    Task<IEnumerable<UbicacionPermitidaResponse>> GetAllAsync();
    Task<UbicacionPermitidaResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateUbicacionPermitidaRequest request);
    Task UpdateAsync(UpdateUbicacionPermitidaRequest request);
    Task DeleteAsync(int id);
}
