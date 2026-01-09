using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IEmpresaService
{
    Task<Result<IEnumerable<EmpresaResponse>>> GetAllAsync();
    Task<Result<EmpresaResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateEmpresaRequest request);
    Task<Result> UpdateAsync(UpdateEmpresaRequest request);
    Task<Result> DeleteAsync(int id);
}
