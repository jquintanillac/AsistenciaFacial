using System.Text;
using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Infrastructure.Data;
using Asistencia.Infrastructure.Repositories;
using Asistencia.Shared.DTOs;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Asistencia.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure
        services.AddSingleton<IDbConnectionFactory, DapperContext>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<DataSeeder>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IHorarioRepository, HorarioRepository>();
        services.AddScoped<IMarcacionRepository, MarcacionRepository>();
        services.AddScoped<IRegistroAsistenciaRepository, RegistroAsistenciaRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IRolUsuarioRepository, RolUsuarioRepository>();
        services.AddScoped<IEnrolamientoRepository, EnrolamientoRepository>();
        services.AddScoped<IHorarioEmpleadoRepository, HorarioEmpleadoRepository>();
        services.AddScoped<IUbicacionPermitidaRepository, UbicacionPermitidaRepository>();
        services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

        // Cache & Blacklist
        services.AddMemoryCache();
        services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
        
        // Services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IRolUsuarioService, RolUsuarioService>();
        services.AddScoped<IHorarioService, HorarioService>();
        services.AddScoped<IMarcacionService, MarcacionService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEnrolamientoService, EnrolamientoService>();
        services.AddScoped<IHorarioEmpleadoService, HorarioEmpleadoService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();
        services.AddScoped<IUbicacionPermitidaService, UbicacionPermitidaService>();
        services.AddScoped<IConfiguracionService, ConfiguracionService>();
        services.AddScoped<IRegistroAsistenciaService, RegistroAsistenciaService>();

        // Shared (Validators)
        services.AddScoped<IValidator<CreateEmployeeRequest>, CreateEmployeeRequestValidator>();
        services.AddScoped<IValidator<UpdateEmployeeRequest>, UpdateEmployeeRequestValidator>();
        services.AddScoped<IValidator<CreateEmpresaRequest>, CreateEmpresaRequestValidator>();
        services.AddScoped<IValidator<UpdateEmpresaRequest>, UpdateEmpresaRequestValidator>();
        services.AddScoped<IValidator<CreateHorarioRequest>, CreateHorarioRequestValidator>();
        services.AddScoped<IValidator<UpdateHorarioRequest>, UpdateHorarioRequestValidator>();
        services.AddScoped<IValidator<CreateUsuarioRequest>, CreateUsuarioRequestValidator>();
        services.AddScoped<IValidator<UpdateUsuarioRequest>, UpdateUsuarioRequestValidator>();
        services.AddScoped<IValidator<CreateMarcacionRequest>, CreateMarcacionRequestValidator>();
        services.AddScoped<IValidator<UpdateMarcacionRequest>, UpdateMarcacionRequestValidator>();
        services.AddScoped<IValidator<CreateRegistroAsistenciaRequest>, CreateRegistroAsistenciaRequestValidator>();
        services.AddScoped<IValidator<UpdateRegistroAsistenciaRequest>, UpdateRegistroAsistenciaRequestValidator>();
        services.AddScoped<IValidator<CreateRolRequest>, CreateRolRequestValidator>();
        services.AddScoped<IValidator<UpdateRolRequest>, UpdateRolRequestValidator>();
        services.AddScoped<IValidator<CreateRolUsuarioRequest>, CreateRolUsuarioRequestValidator>();
        services.AddScoped<IValidator<CreateEnrolamientoRequest>, CreateEnrolamientoRequestValidator>();
        services.AddScoped<IValidator<UpdateEnrolamientoRequest>, UpdateEnrolamientoRequestValidator>();
        services.AddScoped<IValidator<CreateHorarioEmpleadoRequest>, CreateHorarioEmpleadoRequestValidator>();
        services.AddScoped<IValidator<UpdateHorarioEmpleadoRequest>, UpdateHorarioEmpleadoRequestValidator>();
        services.AddScoped<IValidator<CreateUbicacionPermitidaRequest>, CreateUbicacionPermitidaRequestValidator>();
        services.AddScoped<IValidator<UpdateUbicacionPermitidaRequest>, UpdateUbicacionPermitidaRequestValidator>();
        services.AddScoped<IValidator<CreateConfiguracionRequest>, CreateConfiguracionRequestValidator>();
        services.AddScoped<IValidator<UpdateConfiguracionRequest>, UpdateConfiguracionRequestValidator>();
        services.AddScoped<IValidator<CreateAuditoriaRequest>, CreateAuditoriaRequestValidator>();
        services.AddScoped<IValidator<UpdateAuditoriaRequest>, UpdateAuditoriaRequestValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        
        // Add Controllers
        services.AddControllers();
        
        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        // Add Authentication (JWT)
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });
        
        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asistencia API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}
