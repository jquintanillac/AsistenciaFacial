using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class ConfiguracionRepository : IConfiguracionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<ConfiguracionRepository> _logger;

    public ConfiguracionRepository(IDbConnectionFactory connectionFactory, ILogger<ConfiguracionRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<ConfiguracionResponse>> GetAllAsync()
    {
        var query = "sp_Configuracion_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ConfiguracionResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<ConfiguracionResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Configuracion_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdConfiguracion", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ConfiguracionResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateConfiguracionRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Configuracion_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("Clave", request.Clave);
            parameters.Add("Valor", request.Valor);
            parameters.Add("Logo", request.Logo);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding configuracion");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateConfiguracionRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Configuracion_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdConfiguracion", request.IdConfiguracion);
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("Clave", request.Clave);
            parameters.Add("Valor", request.Valor);
            parameters.Add("Logo", request.Logo);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating configuracion");
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
            var query = "sp_Configuracion_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdConfiguracion", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting configuracion");
            throw;
        }
    }
}
