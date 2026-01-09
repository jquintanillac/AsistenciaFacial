using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class HorarioEmpleadoRepository : IHorarioEmpleadoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<HorarioEmpleadoRepository> _logger;

    public HorarioEmpleadoRepository(IDbConnectionFactory connectionFactory, ILogger<HorarioEmpleadoRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<HorarioEmpleadoResponse>> GetAllAsync()
    {
        var query = "sp_HorarioEmpleado_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<HorarioEmpleadoResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<HorarioEmpleadoResponse?> GetByIdAsync(int id)
    {
        var query = "sp_HorarioEmpleado_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdHorarioEmpleado", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<HorarioEmpleadoResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateHorarioEmpleadoRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_HorarioEmpleado_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("IdHorario", request.IdHorario);
            parameters.Add("FechaInicio", request.FechaInicio);
            parameters.Add("FechaFin", request.FechaFin);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding horario empleado");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateHorarioEmpleadoRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_HorarioEmpleado_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdHorarioEmpleado", request.IdHorarioEmpleado);
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("IdHorario", request.IdHorario);
            parameters.Add("FechaInicio", request.FechaInicio);
            parameters.Add("FechaFin", request.FechaFin);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating horario empleado");
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
            var query = "sp_HorarioEmpleado_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdHorarioEmpleado", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting horario empleado");
            throw;
        }
    }
}
