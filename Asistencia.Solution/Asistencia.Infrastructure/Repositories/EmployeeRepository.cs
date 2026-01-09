using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(IDbConnectionFactory connectionFactory, ILogger<EmployeeRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
    {
        var query = "sp_Empleado_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<EmployeeResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<EmployeeResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Empleado_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpleado", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<EmployeeResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateEmployeeRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Empleado_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("DNI", request.DNI);
            parameters.Add("Nombres", request.Nombres);
            parameters.Add("Apellidos", request.Apellidos);
            parameters.Add("Cargo", request.Cargo);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding employee");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateEmployeeRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Empleado_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("DNI", request.DNI);
            parameters.Add("Nombres", request.Nombres);
            parameters.Add("Apellidos", request.Apellidos);
            parameters.Add("Cargo", request.Cargo);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating employee");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Empleado_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting employee");
            throw;
        }
    }
}
