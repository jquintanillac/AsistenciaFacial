using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Net.Http.Json;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.LoadTests;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Load Test...");

        // 1. Obtain Token (Pre-check)
        string? authToken = null;
        string baseUrl = "http://localhost:5247"; // Adjust if needed

        using (var authClient = new HttpClient())
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = "admin",
                    Password = "Admin123*"
                };

                var response = await authClient.PostAsJsonAsync($"{baseUrl}/api/Auth/login", loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    authToken = result?.Token;
                    Console.WriteLine("Authentication successful. Token obtained.");
                }
                else
                {
                    Console.WriteLine($"Authentication failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to API at {baseUrl}. Ensure it is running.");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        if (string.IsNullOrEmpty(authToken))
        {
            Console.WriteLine("Skipping load test execution due to missing token.");
            return;
        }

        // 2. Define NBomber Scenario
        var httpClient = new HttpClient();

        var scenario = Scenario.Create("asistencia_api_load", async context =>
        {
            var stepResponse = await Step.Run("fetch_empresas", context, async () =>
            {
                var request = Http.CreateRequest("GET", $"{baseUrl}/api/Empresa")
                    .WithHeader("Authorization", $"Bearer {authToken}");

                var response = await Http.Send(httpClient, request);
                return response;
            });
            return stepResponse;
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(15))
        );

        // 3. Run
        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}
