using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IConfiguracionRepository
{
    Task<IEnumerable<ConfiguracionResponse>> GetAllAsync();
    Task<ConfiguracionResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateConfiguracionRequest request);
    Task UpdateAsync(UpdateConfiguracionRequest request);
    Task DeleteAsync(int id);
}
