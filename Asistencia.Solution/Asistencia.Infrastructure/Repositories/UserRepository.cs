using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Asistencia.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(IDbConnectionFactory connectionFactory, IPasswordHasher passwordHasher, ILogger<UserRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<UsuarioResponse?> GetByUsernameAsync(string username)
    {
        var query = "SELECT IdUsuario, IdEmpleado, Username, Email, Estado FROM Usuario WHERE Username = @Username AND Estado = 1"; 
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UsuarioResponse>(query, new { Username = username });
    }

    public async Task<UsuarioResponse?> GetByEmailAsync(string email)
    {
        var query = "SELECT IdUsuario, IdEmpleado, Username, Email, Estado FROM Usuario WHERE Email = @Email AND Estado = 1"; 
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UsuarioResponse>(query, new { Email = email });
    }

    public async Task<string?> GetPasswordHashAsync(string username)
    {
        var query = "SELECT PasswordHash FROM Usuario WHERE Username = @Username AND Estado = 1";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<string>(query, new { Username = username });
    }

    public async Task<IEnumerable<UsuarioResponse>> GetAllAsync()
    {
        var query = "sp_Usuario_GETALL";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<UsuarioResponse>(query, commandType: CommandType.StoredProcedure);
    }

    public async Task<UsuarioResponse?> GetByIdAsync(int id)
    {
        var query = "sp_Usuario_GETBYID";
        var parameters = new DynamicParameters();
        parameters.Add("IdUsuario", id);

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UsuarioResponse>(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(CreateUsuarioRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Usuario_INSERT";
            var parameters = new DynamicParameters();
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Username", request.Username);
            parameters.Add("Email", request.Email);
            parameters.Add("PasswordHash", _passwordHasher.Hash(request.Password));

            var id = await connection.QuerySingleAsync<int>(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
            return id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error adding usuario");
            throw;
        }
    }

    public async Task UpdateAsync(UpdateUsuarioRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            string passwordHash;
            if (!string.IsNullOrEmpty(request.Password))
            {
                passwordHash = _passwordHasher.Hash(request.Password);
            }
            else
            {
                // Retrieve existing hash to keep it
                // Ideally SP should handle NULL to keep existing, but let's be safe
                var currentHashQuery = "SELECT PasswordHash FROM Usuario WHERE IdUsuario = @IdUsuario";
                passwordHash = await connection.QuerySingleAsync<string>(currentHashQuery, new { request.IdUsuario }, transaction);
            }

            var query = "sp_Usuario_UPDATE";
            var parameters = new DynamicParameters();
            parameters.Add("IdUsuario", request.IdUsuario);
            parameters.Add("IdEmpleado", request.IdEmpleado);
            parameters.Add("Username", request.Username);
            parameters.Add("PasswordHash", passwordHash);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating usuario");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var query = "sp_Usuario_DELETE";
            var parameters = new DynamicParameters();
            parameters.Add("IdUsuario", id);

            await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting usuario");
            throw;
        }
    }
}
