using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.Logging;

namespace Asistencia.Infrastructure.Data;

public class DataSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IRolUsuarioRepository _rolUsuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        IUserRepository userRepository,
        IRolRepository rolRepository,
        IRolUsuarioRepository rolUsuarioRepository,
        IPasswordHasher passwordHasher,
        ILogger<DataSeeder> logger)
    {
        _userRepository = userRepository;
        _rolRepository = rolRepository;
        _rolUsuarioRepository = rolUsuarioRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Iniciando Seeder...");

            // 1. Ensure Roles exist
            var roles = await _rolRepository.GetAllAsync();
            
            var adminRoleId = await EnsureRoleExists(roles, "Administrador", "Acceso total al sistema y configuración");
            await EnsureRoleExists(roles, "Supervisor", "Gestión de equipos, horarios y aprobación de asistencia");
            await EnsureRoleExists(roles, "Colaborador", "Registro de asistencia y consulta de historial propio");

            // 2. Create Users (Assign 'Administrador' role)
            await CreateUserIfNotExists("admin", "admin@asistencia.com", "Admin123*", adminRoleId);
            await CreateUserIfNotExists("admin2", "admin2@asistencia.com", "Admin2*123", adminRoleId);

            _logger.LogInformation("Seeder completado exitosamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ejecutando el Seeder");
            throw;
        }
    }

    private async Task<int> EnsureRoleExists(IEnumerable<Asistencia.Shared.DTOs.Responses.RolResponse> roles, string nombre, string descripcion)
    {
        var role = roles.FirstOrDefault(r => r.Nombre == nombre);
        if (role == null)
        {
            _logger.LogInformation($"Creando rol {nombre}...");
            return await _rolRepository.AddAsync(new CreateRolRequest 
            { 
                Nombre = nombre, 
                Descripcion = descripcion 
            });
        }
        return role.IdRol;
    }

    private async Task CreateUserIfNotExists(string username, string email, string password, int roleId)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser == null)
        {
            _logger.LogInformation($"Creando usuario {username}...");
            var userId = await _userRepository.AddAsync(new CreateUsuarioRequest
            {
                Username = username,
                Email = email,
                Password = password, 
                IdEmpleado = 0 
            });

            // Assign Role
            await _rolUsuarioRepository.AddAsync(new CreateRolUsuarioRequest
            {
                IdUsuario = userId,
                IdRol = roleId
            });
        }
        else
        {
            _logger.LogInformation($"Usuario {username} ya existe.");
        }
    }
}
