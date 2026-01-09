using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeResponse>> GetAllAsync();
    Task<EmployeeResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateEmployeeRequest request);
    Task UpdateAsync(UpdateEmployeeRequest request);
    Task DeleteAsync(int id);
}
