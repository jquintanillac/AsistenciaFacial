using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class RegistroAsistenciaIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_And_Get_RegistroAsistencia_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var empresaService = scope.ServiceProvider.GetRequiredService<IEmpresaService>();
        var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
        var registroService = scope.ServiceProvider.GetRequiredService<IRegistroAsistenciaService>();

        // 1. Create Empresa
        var random = new Random();
        var ruc = "20" + random.Next(100000000, 999999999).ToString();
        var createEmpresaRequest = new CreateEmpresaRequest
        {
            RazonSocial = $"Registro Test Company {Guid.NewGuid()}",
            RUC = ruc,
            Direccion = "Test Address",
            Telefono = "123456789",
            Email = $"test_registro_{Guid.NewGuid()}@company.com"
        };
        var empresaResult = await empresaService.CreateAsync(createEmpresaRequest);
        Assert.True(empresaResult.IsSuccess, "Empresa creation failed");
        var empresaId = empresaResult.Value;

        // 2. Create Employee
        var dni = random.Next(10000000, 99999999).ToString();
        var createEmployeeRequest = new CreateEmployeeRequest
        {
            IdEmpresa = empresaId,
            Nombres = "Juan",
            Apellidos = "Perez",
            DNI = dni,
            Cargo = "Developer"
        };
        var employeeResult = await employeeService.CreateAsync(createEmployeeRequest);
        Assert.True(employeeResult.IsSuccess, "Employee creation failed");
        var employeeId = employeeResult.Value;

        // 3. Create RegistroAsistencia
        var fecha = DateTime.Now.Date;
        var createRegistroRequest = new CreateRegistroAsistenciaRequest
        {
            IdEmpleado = employeeId,
            Fecha = fecha,
            HoraEntrada = DateTime.Now.Date.AddHours(8),
            HoraSalida = DateTime.Now.Date.AddHours(17),
            MinutosTarde = 0,
            HorasTrabajadas = 8,
            EstadoAsistencia = "Presente"
        };

        var createResult = await registroService.CreateAsync(createRegistroRequest);
        Assert.True(createResult.IsSuccess, $"Registro creation failed: {createResult.Error}");
        var registroId = createResult.Value;
        Assert.True(registroId > 0);

        // 4. GetById
        var getResult = await registroService.GetByIdAsync(registroId);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(createRegistroRequest.Fecha, getResult.Value.Fecha);
        Assert.Equal(createRegistroRequest.EstadoAsistencia, getResult.Value.EstadoAsistencia);

        // 5. Update
        var updateRequest = new UpdateRegistroAsistenciaRequest
        {
            IdRegistro = registroId,
            IdEmpleado = employeeId,
            Fecha = fecha,
            HoraEntrada = DateTime.Now.Date.AddHours(8),
            HoraSalida = DateTime.Now.Date.AddHours(18), // Changed
            MinutosTarde = 0,
            HorasTrabajadas = 9, // Changed
            EstadoAsistencia = "Presente"
        };
        var updateResult = await registroService.UpdateAsync(updateRequest);
        Assert.True(updateResult.IsSuccess);

        var getUpdatedResult = await registroService.GetByIdAsync(registroId);
        Assert.True(getUpdatedResult.IsSuccess);
        Assert.Equal(9, getUpdatedResult.Value.HorasTrabajadas);

        // 6. Cleanup
        await registroService.DeleteAsync(registroId);
        await employeeService.DeleteAsync(employeeId);
        await empresaService.DeleteAsync(empresaId);
    }
}
