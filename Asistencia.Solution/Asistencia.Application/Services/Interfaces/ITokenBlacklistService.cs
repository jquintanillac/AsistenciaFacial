namespace Asistencia.Application.Services;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token, TimeSpan duration);
    Task<bool> IsTokenBlacklistedAsync(string token);
}
