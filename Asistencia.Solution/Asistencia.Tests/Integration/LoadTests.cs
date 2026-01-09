using System.Diagnostics;
using System.Net.Http.Json;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asistencia.Tests.Integration;

public class LoadTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public LoadTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task Login_Endpoint_Should_Handle_Concurrent_Requests()
    {
        // Arrange
        int concurrentRequests = 20; // Simulate 20 concurrent logins
        var client = _factory.CreateClient();
        
        // Register a user first to login with
        var username = $"loaduser_{Guid.NewGuid()}";
        var password = "Password123!";
        var registerRequest = new RegisterRequest
        {
            Username = username,
            Email = $"{username}@test.com",
            Password = password,
            ConfirmPassword = password
        };
        
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", registerRequest);
        registerResponse.EnsureSuccessStatusCode();

        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var tasks = new List<Task<HttpResponseMessage>>();
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(client.PostAsJsonAsync("/api/auth/login", loginRequest));
        }

        var responses = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        var successfulResponses = responses.Count(r => r.IsSuccessStatusCode);
        var averageTime = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;

        _output.WriteLine($"Total Requests: {concurrentRequests}");
        _output.WriteLine($"Successful Requests: {successfulResponses}");
        _output.WriteLine($"Total Time: {stopwatch.ElapsedMilliseconds} ms");
        _output.WriteLine($"Average Time per Request: {averageTime} ms");

        Assert.Equal(concurrentRequests, successfulResponses);
        Assert.True(averageTime < 500, $"Average response time {averageTime}ms is too high (expected < 500ms)");
    }
}
