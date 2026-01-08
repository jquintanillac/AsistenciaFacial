using Asistencia.Shared.DTOs;

namespace Asistencia.Application.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<int> AddAsync(EmployeeDto employee);
    Task UpdateAsync(EmployeeDto employee);
    Task DeleteAsync(int id);
}
