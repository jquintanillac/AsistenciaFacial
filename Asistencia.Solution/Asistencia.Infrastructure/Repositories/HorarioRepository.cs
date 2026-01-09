using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class HorarioRepository : IHorarioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<HorarioRepository> _logger;

    public HorarioRepository(IDbConnectionFactory connectionFactory, ILogger<HorarioRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<HorarioResponse>> GetAllAsync()
    {
        var query = "sp_Horario_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<HorarioResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<HorarioResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Horario_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdHorario", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<HorarioResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateHorarioRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Horario_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("HoraEntrada", request.HoraEntrada);
            parameters.Add("HoraSalida", request.HoraSalida);
            parameters.Add("ToleranciaEntrada", request.ToleranciaEntrada);
            parameters.Add("ToleranciaSalida", request.ToleranciaSalida);
            parameters.Add("IdEmpresa", request.IdEmpresa);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding horario");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateHorarioRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Horario_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdHorario", request.IdHorario);
            parameters.Add("Nombre", request.Nombre);
            parameters.Add("HoraEntrada", request.HoraEntrada);
            parameters.Add("HoraSalida", request.HoraSalida);
            parameters.Add("ToleranciaEntrada", request.ToleranciaEntrada);
            parameters.Add("ToleranciaSalida", request.ToleranciaSalida);
            parameters.Add("IdEmpresa", request.IdEmpresa);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating horario");
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
            var query = "sp_Horario_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdHorario", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting horario");
            throw;
        }
    }
}
