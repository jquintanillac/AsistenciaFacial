using Asistencia.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace Asistencia.Infrastructure.Data;

public class DapperContext : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException("DefaultConnection string is not configured");
    }

    public DbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
