using Asistencia.Application.Common;
using Asistencia.Shared.DTOs;

namespace Asistencia.Application.Services;

public interface IEmployeeService
{
    Task<Result<IEnumerable<EmployeeDto>>> GetAllAsync();
    Task<Result<EmployeeDto>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(EmployeeDto employee);
}
