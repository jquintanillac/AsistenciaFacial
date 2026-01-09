using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IRegistroAsistenciaRepository
{
    Task<IEnumerable<RegistroAsistenciaResponse>> GetAllAsync();
    Task<RegistroAsistenciaResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateRegistroAsistenciaRequest request);
    Task UpdateAsync(UpdateRegistroAsistenciaRequest request);
    Task DeleteAsync(int id);
}
