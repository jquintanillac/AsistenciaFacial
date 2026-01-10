using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Asistencia.Web.Services;

public class JwtHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                // Ensure token is clean (remove potential extra quotes)
                token = token.Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[JwtHeaderHandler] Attaching token to request: {request.RequestUri}");
            }
            else
            {
                Console.WriteLine($"[JwtHeaderHandler] No token found for request: {request.RequestUri}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[JwtHeaderHandler] Error accessing localStorage: {ex.Message}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"[JwtHeaderHandler] 401 Unauthorized received from: {request.RequestUri}");
        }

        return response;
    }
}
