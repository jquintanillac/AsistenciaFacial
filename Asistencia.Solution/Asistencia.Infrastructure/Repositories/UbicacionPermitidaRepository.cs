using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class UbicacionPermitidaRepository : IUbicacionPermitidaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<UbicacionPermitidaRepository> _logger;

    public UbicacionPermitidaRepository(IDbConnectionFactory connectionFactory, ILogger<UbicacionPermitidaRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<UbicacionPermitidaResponse>> GetAllAsync()
    {
        var query = "sp_UbicacionPermitida_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<UbicacionPermitidaResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<UbicacionPermitidaResponse?> GetByIdAsync(int id)
    {
        var query = "sp_UbicacionPermitida_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdUbicacion", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UbicacionPermitidaResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateUbicacionPermitidaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_UbicacionPermitida_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("Latitud", request.Latitud);
            parameters.Add("Longitud", request.Longitud);
            parameters.Add("RadioMetros", request.RadioMetros);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding ubicacion permitida");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateUbicacionPermitidaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_UbicacionPermitida_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdUbicacion", request.IdUbicacion);
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("Latitud", request.Latitud);
            parameters.Add("Longitud", request.Longitud);
            parameters.Add("RadioMetros", request.RadioMetros);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating ubicacion permitida");
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
            var query = "sp_UbicacionPermitida_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdUbicacion", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting ubicacion permitida");
            throw;
        }
    }
}
