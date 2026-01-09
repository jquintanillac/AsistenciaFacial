using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IMarcacionService
{
    Task<Result<IEnumerable<MarcacionResponse>>> GetAllAsync();
    Task<Result<MarcacionResponse>> GetByIdAsync(long id);
    Task<Result<IEnumerable<MarcacionResponse>>> GetByEmployeeIdAsync(int employeeId);
    Task<Result<int>> CreateAsync(CreateMarcacionRequest request);
    Task<Result> UpdateAsync(UpdateMarcacionRequest request);
    Task<Result> DeleteAsync(long id);
}
