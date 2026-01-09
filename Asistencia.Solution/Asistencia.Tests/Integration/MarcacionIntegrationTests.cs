using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class MarcacionIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_And_Get_Marcacion_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var empresaService = scope.ServiceProvider.GetRequiredService<IEmpresaService>();
        var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        var marcacionService = scope.ServiceProvider.GetRequiredService<IMarcacionService>();

        // 1. Create Empresa
        var random = new Random();
        var ruc = "20" + random.Next(100000000, 999999999).ToString();
        var createEmpresaRequest = new CreateEmpresaRequest
        {
            RazonSocial = $"Marcacion Test Company {Guid.NewGuid()}",
            RUC = ruc,
            Direccion = "Test Address",
            Telefono = "123456789",
            Email = $"test_marcacion_{Guid.NewGuid()}@company.com"
        };
        var empresaResult = await empresaService.CreateAsync(createEmpresaRequest);
        Assert.True(empresaResult.IsSuccess, "Empresa creation failed");
        var empresaId = empresaResult.Value;

        // 2. Create Employee
        var dni = random.Next(10000000, 99999999).ToString();
        var createEmployeeRequest = new CreateEmployeeRequest
        {
            IdEmpresa = empresaId,
            DNI = dni,
            Nombres = "Test",
            Apellidos = "Employee",
            Cargo = "Tester"
        };
        var employeeResult = await employeeService.CreateAsync(createEmployeeRequest);
        Assert.True(employeeResult.IsSuccess, "Employee creation failed");
        var employeeId = employeeResult.Value;

        // 3. Create Marcacion
        var createMarcacionRequest = new CreateMarcacionRequest
        {
            IdEmpleado = employeeId,
            FechaHora = DateTime.Now,
            TipoMarcacion = "E", // Entrada
            Latitud = -12.0m,
            Longitud = -77.0m,
            EsValida = true
        };
        var marcacionResult = await marcacionService.CreateAsync(createMarcacionRequest);
        Assert.True(marcacionResult.IsSuccess, $"Marcacion creation failed: {marcacionResult.Error}");
        var marcacionId = marcacionResult.Value;
        Assert.True(marcacionId > 0);

        // 4. Get Marcacion
        var getResult = await marcacionService.GetByIdAsync(marcacionId);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(createMarcacionRequest.IdEmpleado, getResult.Value.IdEmpleado);
        Assert.Equal(createMarcacionRequest.TipoMarcacion, getResult.Value.TipoMarcacion);

        // 5. Cleanup
        await marcacionService.DeleteAsync(marcacionId);
        await employeeService.DeleteAsync(employeeId);
        await empresaService.DeleteAsync(empresaId);
    }
}