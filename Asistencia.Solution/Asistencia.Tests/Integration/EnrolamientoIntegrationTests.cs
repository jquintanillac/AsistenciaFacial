using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class EnrolamientoIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_And_Get_Enrolamiento_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var empresaService = scope.ServiceProvider.GetRequiredService<IEmpresaService>();
        var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        var enrolamientoService = scope.ServiceProvider.GetRequiredService<IEnrolamientoService>();

        // 1. Create Empresa
        var random = new Random();
        var ruc = "20" + random.Next(100000000, 999999999).ToString();
        var createEmpresaRequest = new CreateEmpresaRequest
        {
            RazonSocial = $"Enrolamiento Test Company {Guid.NewGuid()}",
            RUC = ruc,
            Direccion = "Test Address",
            Telefono = "123456789",
            Email = $"test_enrol_{Guid.NewGuid()}@company.com"
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
            Nombres = "Test Enrol",
            Apellidos = "Employee",
            Cargo = "Tester"
        };
        var employeeResult = await employeeService.CreateAsync(createEmployeeRequest);
        Assert.True(employeeResult.IsSuccess, "Employee creation failed");
        var employeeId = employeeResult.Value;

        // 3. Create Enrolamiento
        var createEnrolamientoRequest = new CreateEnrolamientoRequest
        {
            IdEmpleado = employeeId,
            Tipo = "Huella",
            IdentificadorBiometrico = new byte[] { 0x01, 0x02, 0x03, 0x04 }
        };

        var createResult = await enrolamientoService.CreateAsync(createEnrolamientoRequest);
        Assert.True(createResult.IsSuccess, $"Enrolamiento creation failed: {createResult.Error}");
        var enrolamientoId = createResult.Value;
        Assert.True(enrolamientoId > 0);

        // 4. GetById
        var getResult = await enrolamientoService.GetByIdAsync(enrolamientoId);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(createEnrolamientoRequest.Tipo, getResult.Value.Tipo);
        Assert.Equal(createEnrolamientoRequest.IdentificadorBiometrico, getResult.Value.IdentificadorBiometrico);

        // 5. Cleanup
        await enrolamientoService.DeleteAsync(enrolamientoId);
        await employeeService.DeleteAsync(employeeId);
        await empresaService.DeleteAsync(empresaId);
    }
}