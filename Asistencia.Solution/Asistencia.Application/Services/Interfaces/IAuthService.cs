using Asistencia.Application.Common;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;

namespace Asistencia.Application.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<int>> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> LogoutAsync(string token);
}
