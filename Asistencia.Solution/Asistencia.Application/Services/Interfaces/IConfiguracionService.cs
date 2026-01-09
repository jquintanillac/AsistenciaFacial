using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IConfiguracionService
{
    Task<Result<IEnumerable<ConfiguracionResponse>>> GetAllAsync();
    Task<Result<ConfiguracionResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateConfiguracionRequest request);
    Task<Result> UpdateAsync(UpdateConfiguracionRequest request);
    Task<Result> DeleteAsync(int id);
}
