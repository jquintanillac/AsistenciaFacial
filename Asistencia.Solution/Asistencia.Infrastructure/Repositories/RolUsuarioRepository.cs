using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class RolUsuarioRepository : IRolUsuarioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<RolUsuarioRepository> _logger;

    public RolUsuarioRepository(IDbConnectionFactory connectionFactory, ILogger<RolUsuarioRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<RolUsuarioResponse>> GetAllAsync()
    {
        var query = "sp_RolUsuario_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RolUsuarioResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<RolUsuarioResponse>> GetByUserIdAsync(int idUsuario)
    {
        var query = "sp_RolUsuario_GETBYUSER";
        var parameters = new DynamicParameters();
        parameters.Add("IdUsuario", idUsuario);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RolUsuarioResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task AddAsync(CreateRolUsuarioRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_RolUsuario_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdUsuario", request.IdUsuario);
            parameters.Add("IdRol", request.IdRol);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding rol usuario");
            throw;
        }
    }

    public async Task DeleteAsync(int idUsuario, int idRol)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_RolUsuario_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdUsuario", idUsuario);
            parameters.Add("IdRol", idRol);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting rol usuario");
            throw;
        }
    }
}
