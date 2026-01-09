using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IEmpresaRepository
{
    Task<IEnumerable<EmpresaResponse>> GetAllAsync();
    Task<EmpresaResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateEmpresaRequest request);
    Task UpdateAsync(UpdateEmpresaRequest request);
    Task DeleteAsync(int id);
}
