using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Infrastructure.Data;
using Asistencia.Infrastructure.Repositories;
using Asistencia.Shared.DTOs;
using Asistencia.Shared.Validators;
using FluentValidation;

namespace Asistencia.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure
        services.AddSingleton<DapperContext>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        // Application
        services.AddScoped<IEmployeeService, EmployeeService>();

        // Shared (Validators)
        services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();
        
        // Add Controllers
        services.AddControllers();
        
        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
