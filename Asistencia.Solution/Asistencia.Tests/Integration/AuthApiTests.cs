using System.Net.Http.Json;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Asistencia.Tests.Integration;

public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Login_Refresh_Logout_Flow()
    {
        // 1. Register
        var random = new Random();
        var username = $"user_{Guid.NewGuid()}";
        var email = $"{username}@test.com";
        var password = "Password123!";

        var registerRequest = new RegisterRequest
        {
            Username = username,
            Email = email,
            Password = password,
            ConfirmPassword = password
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();

        // 2. Login
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResult);
        Assert.False(string.IsNullOrEmpty(loginResult.Token));
        Assert.False(string.IsNullOrEmpty(loginResult.RefreshToken));

        // 3. Access Protected Endpoint
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);
        var protectedResponse = await _client.GetAsync("/api/empresa"); 
        Assert.Equal(System.Net.HttpStatusCode.OK, protectedResponse.StatusCode);

        // 4. Refresh Token
        // Wait 1 second to ensure token expiration changes
        await Task.Delay(1000);

        var refreshRequest = new RefreshTokenRequest
        {
            Token = loginResult.Token,
            RefreshToken = loginResult.RefreshToken
        };
        var refreshResponse = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshRequest);
        refreshResponse.EnsureSuccessStatusCode();
        var refreshResult = await refreshResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(refreshResult);
        Assert.NotEqual(loginResult.Token, refreshResult.Token);
        Assert.NotEqual(loginResult.RefreshToken, refreshResult.RefreshToken);

        // 5. Logout
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshResult.Token);
        var logoutResponse = await _client.PostAsync("/api/auth/logout", null);
        logoutResponse.EnsureSuccessStatusCode();

        // 6. Access Protected Endpoint (Blacklisted)
        var deniedResponse = await _client.GetAsync("/api/empresa");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, deniedResponse.StatusCode);
    }
}
