using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class EmpresaIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Create_And_Get_Empresa_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IEmpresaService>();

        // 1. Create
        var random = new Random();
        var ruc = "20" + random.Next(100000000, 999999999).ToString();
        
        var createRequest = new CreateEmpresaRequest
        {
            RazonSocial = $"Integration Test Company {Guid.NewGuid()}",
            RUC = ruc,
            Direccion = "Test Address",
            Telefono = "123456789",
            Email = $"test_{Guid.NewGuid()}@company.com"
        };
        
        var createResult = await service.CreateAsync(createRequest);
        Assert.True(createResult.IsSuccess, $"Create failed: {createResult.Error}");
        var id = createResult.Value;
        Assert.True(id > 0);

        // 2. GetById
        var getResult = await service.GetByIdAsync(id);
        Assert.True(getResult.IsSuccess);
        Assert.Equal(createRequest.RazonSocial, getResult.Value.RazonSocial);

        // 3. Cleanup (Delete)
        var deleteResult = await service.DeleteAsync(id);
        Assert.True(deleteResult.IsSuccess);

        // 4. Verify Delete
        var getAfterDelete = await service.GetByIdAsync(id);
        Assert.False(getAfterDelete.IsSuccess); // Should fail not found
    }
}
