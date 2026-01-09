using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Asistencia.Tests.Integration;

public class AuthIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Register_And_Login_Success()
    {
        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var usuarioService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();

        // 1. Register
        var random = new Random();
        var username = $"user_{Guid.NewGuid()}";
        var email = $"test_{Guid.NewGuid()}@example.com";
        var password = "Password123!";

        var registerRequest = new RegisterRequest
        {
            Username = username,
            Email = email,
            Password = password,
            ConfirmPassword = password
        };

        var registerResult = await authService.RegisterAsync(registerRequest);
        Assert.True(registerResult.IsSuccess, $"Register failed: {registerResult.Error}");
        var userId = registerResult.Value;
        Assert.True(userId > 0);

        // 2. Login
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var loginResult = await authService.LoginAsync(loginRequest);
        Assert.True(loginResult.IsSuccess, $"Login failed: {loginResult.Error}");
        Assert.NotNull(loginResult.Value.Token);
        Assert.Equal(username, loginResult.Value.Username);

        // 3. Cleanup
        var deleteResult = await usuarioService.DeleteAsync(userId);
        Assert.True(deleteResult.IsSuccess);
    }
}