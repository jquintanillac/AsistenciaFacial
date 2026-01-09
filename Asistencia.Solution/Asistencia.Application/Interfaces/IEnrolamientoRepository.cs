using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IEnrolamientoRepository
{
    Task<IEnumerable<EnrolamientoResponse>> GetAllAsync();
    Task<EnrolamientoResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateEnrolamientoRequest request);
    Task UpdateAsync(UpdateEnrolamientoRequest request);
    Task DeleteAsync(int id);
}
