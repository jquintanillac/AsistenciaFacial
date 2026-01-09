using Microsoft.Extensions.Caching.Memory;

namespace Asistencia.Application.Services;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;

    public TokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task BlacklistTokenAsync(string token, TimeSpan duration)
    {
        _cache.Set(token, true, duration);
        return Task.CompletedTask;
    }

    public Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return Task.FromResult(_cache.TryGetValue(token, out _));
    }
}
