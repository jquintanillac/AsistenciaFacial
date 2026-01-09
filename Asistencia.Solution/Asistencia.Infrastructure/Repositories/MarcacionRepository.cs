using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class MarcacionRepository : IMarcacionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<MarcacionRepository> _logger;

    public MarcacionRepository(IDbConnectionFactory connectionFactory, ILogger<MarcacionRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<MarcacionResponse>> GetAllAsync()
    {
        var query = "sp_Marcacion_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<MarcacionResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<MarcacionResponse?> GetByIdAsync(long id)
    {
        var query = "sp_Marcacion_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdMarcacion", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<MarcacionResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateMarcacionRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Marcacion_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("FechaHora", request.FechaHora);
            parameters.Add("TipoMarcacion", request.TipoMarcacion);
            parameters.Add("Latitud", request.Latitud);
            parameters.Add("Longitud", request.Longitud);
            parameters.Add("EsValida", request.EsValida);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding marcacion");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateMarcacionRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Marcacion_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdMarcacion", request.IdMarcacion); // Ensure IdMarcacion in UpdateRequest is long? Let's check DTO later.
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("FechaHora", request.FechaHora);
            parameters.Add("TipoMarcacion", request.TipoMarcacion);
            parameters.Add("Latitud", request.Latitud);
            parameters.Add("Longitud", request.Longitud);
            parameters.Add("EsValida", request.EsValida);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating marcacion");
            throw;
        }
    }

    public async Task DeleteAsync(long id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Marcacion_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdMarcacion", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting marcacion");
            throw;
        }
    }
}
