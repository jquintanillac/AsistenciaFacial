using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class HorarioIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_And_Get_Horario_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var empresaService = scope.ServiceProvider.GetRequiredService<IEmpresaService>();
        var horarioService = scope.ServiceProvider.GetRequiredService<IHorarioService>();

        // 1. Create Empresa
        var random = new Random();
        var ruc = "20" + random.Next(100000000, 999999999).ToString();
        var createEmpresaRequest = new CreateEmpresaRequest
        {
            RazonSocial = $"Horario Test Company {Guid.NewGuid()}",
            RUC = ruc,
            Direccion = "Test Address",
            Telefono = "123456789",
            Email = $"test_horario_{Guid.NewGuid()}@company.com"
        };
        var empresaResult = await empresaService.CreateAsync(createEmpresaRequest);
        Assert.True(empresaResult.IsSuccess, "Empresa creation failed");
        var empresaId = empresaResult.Value;

        // 2. Create Horario
        var createHorarioRequest = new CreateHorarioRequest
        {
            IdEmpresa = empresaId,
            Nombre = "Turno Mañana Integration",
            HoraEntrada = new TimeSpan(8, 0, 0),
            HoraSalida = new TimeSpan(17, 0, 0),
            ToleranciaEntrada = 15,
            ToleranciaSalida = 15
        };

        var createResult = await horarioService.CreateAsync(createHorarioRequest);
        Assert.True(createResult.IsSuccess, $"Horario creation failed: {createResult.Error}");
        var horarioId = createResult.Value;
        Assert.True(horarioId > 0);

        // 3. GetById
        var getResult = await horarioService.GetByIdAsync(horarioId);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(createHorarioRequest.Nombre, getResult.Value.Nombre);
        Assert.Equal(createHorarioRequest.HoraEntrada, getResult.Value.HoraEntrada);

        // 4. Cleanup
        await horarioService.DeleteAsync(horarioId);
        await empresaService.DeleteAsync(empresaId);
    }
}