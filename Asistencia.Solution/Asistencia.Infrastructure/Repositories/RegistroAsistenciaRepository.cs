using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class RegistroAsistenciaRepository : IRegistroAsistenciaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<RegistroAsistenciaRepository> _logger;

    public RegistroAsistenciaRepository(IDbConnectionFactory connectionFactory, ILogger<RegistroAsistenciaRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<RegistroAsistenciaResponse>> GetAllAsync()
    {
        var query = "sp_RegistroAsistencia_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RegistroAsistenciaResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<RegistroAsistenciaResponse?> GetByIdAsync(int id)
    {
        var query = "sp_RegistroAsistencia_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdRegistro", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RegistroAsistenciaResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateRegistroAsistenciaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_RegistroAsistencia_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Fecha", request.Fecha);
            parameters.Add("HoraEntrada", request.HoraEntrada);
            parameters.Add("HoraSalida", request.HoraSalida);
            parameters.Add("MinutosTarde", request.MinutosTarde);
            parameters.Add("HorasTrabajadas", request.HorasTrabajadas);
            parameters.Add("EstadoAsistencia", request.EstadoAsistencia);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding registro asistencia");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateRegistroAsistenciaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_RegistroAsistencia_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdRegistro", request.IdRegistro);
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Fecha", request.Fecha);
            parameters.Add("HoraEntrada", request.HoraEntrada);
            parameters.Add("HoraSalida", request.HoraSalida);
            parameters.Add("MinutosTarde", request.MinutosTarde);
            parameters.Add("HorasTrabajadas", request.HorasTrabajadas);
            parameters.Add("EstadoAsistencia", request.EstadoAsistencia);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating registro asistencia");
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
            var query = "sp_RegistroAsistencia_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdRegistro", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting registro asistencia");
            throw;
        }
    }
}
