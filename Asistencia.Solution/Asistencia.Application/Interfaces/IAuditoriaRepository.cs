using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IAuditoriaRepository
{
    Task<IEnumerable<AuditoriaResponse>> GetAllAsync();
    Task<AuditoriaResponse?> GetByIdAsync(long id);
    Task<int> AddAsync(CreateAuditoriaRequest request);
    // Usually Auditoria is read-only or append-only, but adhering to pattern
    Task UpdateAsync(UpdateAuditoriaRequest request);
    Task DeleteAsync(long id);
}
