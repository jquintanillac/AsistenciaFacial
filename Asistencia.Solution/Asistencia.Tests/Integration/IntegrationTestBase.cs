using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Infrastructure.Data;
using Asistencia.Infrastructure.Repositories;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Asistencia.Tests.Integration;

public class IntegrationTestBase
{
    protected IServiceProvider _serviceProvider;

    public IntegrationTestBase()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<IDbConnectionFactory, DapperContext>();
        
        // Add Logging
        services.AddLogging(builder => 
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        // Register Repositories
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IRolUsuarioRepository, RolUsuarioRepository>();
        services.AddScoped<IHorarioRepository, HorarioRepository>();
        services.AddScoped<IMarcacionRepository, MarcacionRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
        services.AddScoped<IEnrolamientoRepository, EnrolamientoRepository>();
        services.AddScoped<IHorarioEmpleadoRepository, HorarioEmpleadoRepository>();
        services.AddScoped<IUbicacionPermitidaRepository, UbicacionPermitidaRepository>();
        services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
        services.AddScoped<IRegistroAsistenciaRepository, RegistroAsistenciaRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Register Caching
        services.AddMemoryCache();
        
        // Register Services
        services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IRolUsuarioService, RolUsuarioService>();
        services.AddScoped<IHorarioService, HorarioService>();
        services.AddScoped<IMarcacionService, MarcacionService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEnrolamientoService, EnrolamientoService>();
        services.AddScoped<IHorarioEmpleadoService, HorarioEmpleadoService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();
        services.AddScoped<IUbicacionPermitidaService, UbicacionPermitidaService>();
        services.AddScoped<IConfiguracionService, ConfiguracionService>();
        services.AddScoped<IRegistroAsistenciaService, RegistroAsistenciaService>();

        // Register Validators
        services.AddScoped<IValidator<CreateEmpresaRequest>, CreateEmpresaRequestValidator>();
        services.AddScoped<IValidator<UpdateEmpresaRequest>, UpdateEmpresaRequestValidator>();
        services.AddScoped<IValidator<CreateRolRequest>, CreateRolRequestValidator>();
        services.AddScoped<IValidator<UpdateRolRequest>, UpdateRolRequestValidator>();
        services.AddScoped<IValidator<CreateRolUsuarioRequest>, CreateRolUsuarioRequestValidator>();
        services.AddScoped<IValidator<CreateHorarioRequest>, CreateHorarioRequestValidator>();
        services.AddScoped<IValidator<UpdateHorarioRequest>, UpdateHorarioRequestValidator>();
        services.AddScoped<IValidator<CreateMarcacionRequest>, CreateMarcacionRequestValidator>();
        services.AddScoped<IValidator<UpdateMarcacionRequest>, UpdateMarcacionRequestValidator>();
        services.AddScoped<IValidator<CreateEmployeeRequest>, CreateEmployeeRequestValidator>();
        services.AddScoped<IValidator<UpdateEmployeeRequest>, UpdateEmployeeRequestValidator>();
        services.AddScoped<IValidator<CreateUsuarioRequest>, CreateUsuarioRequestValidator>();
        services.AddScoped<IValidator<UpdateUsuarioRequest>, UpdateUsuarioRequestValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<IValidator<CreateAuditoriaRequest>, CreateAuditoriaRequestValidator>();
        services.AddScoped<IValidator<UpdateAuditoriaRequest>, UpdateAuditoriaRequestValidator>();
        services.AddScoped<IValidator<CreateEnrolamientoRequest>, CreateEnrolamientoRequestValidator>();
        services.AddScoped<IValidator<UpdateEnrolamientoRequest>, UpdateEnrolamientoRequestValidator>();
        services.AddScoped<IValidator<CreateHorarioEmpleadoRequest>, CreateHorarioEmpleadoRequestValidator>();
        services.AddScoped<IValidator<UpdateHorarioEmpleadoRequest>, UpdateHorarioEmpleadoRequestValidator>();
        services.AddScoped<IValidator<CreateUbicacionPermitidaRequest>, CreateUbicacionPermitidaRequestValidator>();
        services.AddScoped<IValidator<UpdateUbicacionPermitidaRequest>, UpdateUbicacionPermitidaRequestValidator>();
        services.AddScoped<IValidator<CreateConfiguracionRequest>, CreateConfiguracionRequestValidator>();
        services.AddScoped<IValidator<UpdateConfiguracionRequest>, UpdateConfiguracionRequestValidator>();
        services.AddScoped<IValidator<CreateRegistroAsistenciaRequest>, CreateRegistroAsistenciaRequestValidator>();
        services.AddScoped<IValidator<UpdateRegistroAsistenciaRequest>, UpdateRegistroAsistenciaRequestValidator>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
