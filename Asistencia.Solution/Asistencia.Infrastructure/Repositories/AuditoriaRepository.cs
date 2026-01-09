using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<AuditoriaRepository> _logger;

    public AuditoriaRepository(IDbConnectionFactory connectionFactory, ILogger<AuditoriaRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<AuditoriaResponse>> GetAllAsync()
    {
        var query = "sp_Auditoria_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<AuditoriaResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<AuditoriaResponse?> GetByIdAsync(long id)
    {
        var query = "sp_Auditoria_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdAuditoria", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<AuditoriaResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateAuditoriaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        
        try
        {
            var query = "sp_Auditoria_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("Entidad", request.Entidad);
            parameters.Add("Accion", request.Accion);
            parameters.Add("IdUsuario", request.IdUsuario);
            parameters.Add("DetalleAnterior", request.DetalleAnterior);
            parameters.Add("DetalleNuevo", request.DetalleNuevo);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding auditoria");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateAuditoriaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Auditoria_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdAuditoria", request.IdAuditoria);
            parameters.Add("Entidad", request.Entidad);
            parameters.Add("Accion", request.Accion);
            parameters.Add("IdUsuario", request.IdUsuario);
            parameters.Add("DetalleAnterior", request.DetalleAnterior);
            parameters.Add("DetalleNuevo", request.DetalleNuevo);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating auditoria");
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
            var query = "sp_Auditoria_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdAuditoria", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting auditoria");
            throw;
        }
    }
}
