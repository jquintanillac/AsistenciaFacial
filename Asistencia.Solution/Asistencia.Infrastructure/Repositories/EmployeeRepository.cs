using Asistencia.Application.Interfaces;
using Asistencia.Infrastructure.Data;
using Asistencia.Shared.DTOs;
using Dapper;

namespace Asistencia.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperContext _context;

    public EmployeeRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var query = "SELECT * FROM Employees";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<EmployeeDto>(query);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Employees WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<EmployeeDto>(query, new { Id = id });
    }

    public async Task<int> AddAsync(EmployeeDto employee)
    {
        try
        {

        }
        catch (Exception)
        {

            throw;
        }
 
        var query = "INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth) VALUES (@FirstName, @LastName, @Email, @DateOfBirth); SELECT CAST(SCOPE_IDENTITY() as int)";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<int>(query, employee);
    }

    public async Task UpdateAsync(EmployeeDto employee)
    {
        var query = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, employee);
    }

    public async Task DeleteAsync(int id)
    {
        var query = "DELETE FROM Employees WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Id = id });
    }
}
