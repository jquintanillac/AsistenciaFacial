using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<EmpresaRepository> _logger;

    public EmpresaRepository(IDbConnectionFactory connectionFactory, ILogger<EmpresaRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<EmpresaResponse>> GetAllAsync()
    {
        var query = "sp_Empresa_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<EmpresaResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<EmpresaResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Empresa_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdEmpresa", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<EmpresaResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateEmpresaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Empresa_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("RazonSocial", request.RazonSocial);
            parameters.Add("RUC", request.RUC);
            parameters.Add("Email", request.Email);
            parameters.Add("Telefono", request.Telefono);
            parameters.Add("Direccion", request.Direccion);
            parameters.Add("ConfiguracionAsistencia", request.ConfiguracionAsistencia);

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding empresa");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateEmpresaRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Empresa_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpresa", request.IdEmpresa);
            parameters.Add("RazonSocial", request.RazonSocial);
            parameters.Add("RUC", request.RUC);
            parameters.Add("Email", request.Email);
            parameters.Add("Telefono", request.Telefono);
            parameters.Add("Direccion", request.Direccion);
            parameters.Add("ConfiguracionAsistencia", request.ConfiguracionAsistencia);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating empresa");
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
            var query = "sp_Empresa_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpresa", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting empresa");
            throw;
        }
    }
}
