using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class EnrolamientoRepository : IEnrolamientoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<EnrolamientoRepository> _logger;

    public EnrolamientoRepository(IDbConnectionFactory connectionFactory, ILogger<EnrolamientoRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<EnrolamientoResponse>> GetAllAsync()
    {
        var query = "sp_Enrolamiento_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<EnrolamientoResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<EnrolamientoResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Enrolamiento_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdEnrolamiento", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<EnrolamientoResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateEnrolamientoRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Enrolamiento_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Tipo", request.Tipo);
            parameters.Add("IdentificadorBiometrico", request.IdentificadorBiometrico);
            parameters.Add("RutaImagen", request.RutaImagen);
            parameters.Add("DescriptorFacial", request.DescriptorFacial);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding enrolamiento");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateEnrolamientoRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Enrolamiento_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEnrolamiento", request.IdEnrolamiento);
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Tipo", request.Tipo);
            parameters.Add("IdentificadorBiometrico", request.IdentificadorBiometrico);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating enrolamiento");
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
            var query = "sp_Enrolamiento_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEnrolamiento", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting enrolamiento");
            throw;
        }
    }
}
