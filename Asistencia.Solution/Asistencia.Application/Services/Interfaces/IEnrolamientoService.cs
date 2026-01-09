using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IEnrolamientoService
{
    Task<Result<IEnumerable<EnrolamientoResponse>>> GetAllAsync();
    Task<Result<EnrolamientoResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateEnrolamientoRequest request);
    Task<Result> UpdateAsync(UpdateEnrolamientoRequest request);
    Task<Result> DeleteAsync(int id);
}
