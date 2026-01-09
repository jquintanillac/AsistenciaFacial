using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IMarcacionRepository
{
    Task<IEnumerable<MarcacionResponse>> GetAllAsync();
    Task<MarcacionResponse?> GetByIdAsync(long id);
    Task<int> AddAsync(CreateMarcacionRequest request);
    Task UpdateAsync(UpdateMarcacionRequest request);
    Task DeleteAsync(long id);
}
