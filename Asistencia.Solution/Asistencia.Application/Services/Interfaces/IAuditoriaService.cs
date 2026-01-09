using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IAuditoriaService
{
    Task<Result<IEnumerable<AuditoriaResponse>>> GetAllAsync();
    Task<Result<AuditoriaResponse>> GetByIdAsync(long id);
    Task<Result<int>> CreateAsync(CreateAuditoriaRequest request);
    Task<Result> UpdateAsync(UpdateAuditoriaRequest request);
    Task<Result> DeleteAsync(long id);
}
