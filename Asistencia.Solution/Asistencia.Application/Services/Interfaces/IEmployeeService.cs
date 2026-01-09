using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IEmployeeService
{
    Task<Result<IEnumerable<EmployeeResponse>>> GetAllAsync();
    Task<Result<EmployeeResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateEmployeeRequest request);
    Task<Result> UpdateAsync(UpdateEmployeeRequest request);
    Task<Result> DeleteAsync(int id);
}
