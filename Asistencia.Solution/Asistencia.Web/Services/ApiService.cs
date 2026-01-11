using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Asistencia.Application.Common;

namespace Asistencia.Web.Services;

public interface IApiService
{
    Task<Result<T>> GetAsync<T>(string endpoint);
    Task<Result<T>> PostAsync<T>(string endpoint, object data);
    Task<Result<bool>> PostAsync(string endpoint, object data);
    Task<Result<T>> PutAsync<T>(string endpoint, object data);
    Task<Result<bool>> PutAsync(string endpoint, object data);
    Task<Result<T>> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent content);
    Task<Result<bool>> DeleteAsync(string endpoint);
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<Result<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return Result<T>.Success(result!);
            }
            
            Console.WriteLine($"[ApiService] GET {endpoint} failed with {response.StatusCode}");
            var errorContent = await response.Content.ReadAsStringAsync();
            return Result<T>.Failure($"Error {response.StatusCode}: {errorContent}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] Exception in GET {endpoint}: {ex.Message}");
            return Result<T>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<Result<T>> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    public async Task<Result<bool>> PostAsync(string endpoint, object data)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            return await HandleResponseBool(response);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    public async Task<Result<T>> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    public async Task<Result<T>> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    public async Task<Result<bool>> PutAsync(string endpoint, object data)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            return await HandleResponseBool(response);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return await HandleResponseBool(response);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error de conexión: {ex.Message}");
        }
    }

    private async Task<Result<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return Result<T>.Success(default!);
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                 return Result<T>.Success(default!);
            }

            try 
            {
                var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                return Result<T>.Success(result!);
            }
            catch
            {
                // Try to handle wrapped result if T is not Result<T>
                // This part depends on how the API returns data. 
                // Assuming API returns standard JSON matching T.
                 return Result<T>.Success(default!);
            }
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return Result<T>.Failure($"Error {response.StatusCode}: {errorContent}");
    }

    private async Task<Result<bool>> HandleResponseBool(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result<bool>.Success(true);
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return Result<bool>.Failure($"Error {response.StatusCode}: {errorContent}");
    }
}
