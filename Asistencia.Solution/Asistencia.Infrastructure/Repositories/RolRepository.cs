using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class RolRepository : IRolRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<RolRepository> _logger;

    public RolRepository(IDbConnectionFactory connectionFactory, ILogger<RolRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<RolResponse>> GetAllAsync()
    {
        var query = "sp_Rol_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RolResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<RolResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Rol_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdRol", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RolResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateRolRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Rol_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("Descripcion", request.Descripcion);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding rol");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateRolRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Rol_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdRol", request.IdRol);
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("Descripcion", request.Descripcion);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating rol");
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
            var query = "sp_Rol_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdRol", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting rol");
            throw;
        }
    }
}
