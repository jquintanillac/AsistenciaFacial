using Asistencia.Application.Common;
using Asistencia.Application.Common.Security;
using Asistencia.Application.Interfaces;
using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Asistencia.Shared.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Asistencia.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IRolUsuarioRepository _rolUsuarioRepository;
    private readonly IRolRepository _rolRepository;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly IMemoryCache _cache;

    public AuthService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IConfiguration configuration,
        IRolUsuarioRepository rolUsuarioRepository,
        IRolRepository rolRepository,
        ITokenBlacklistService tokenBlacklistService,
        IMemoryCache cache)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _rolUsuarioRepository = rolUsuarioRepository;
        _rolRepository = rolRepository;
        _tokenBlacklistService = tokenBlacklistService;
        _cache = cache;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
            return Result<LoginResponse>.Failure("Usuario o contraseña incorrectos");

        var passwordHash = await _userRepository.GetPasswordHashAsync(request.Username);
        if (passwordHash == null || !_passwordHasher.Verify(request.Password, passwordHash))
             return Result<LoginResponse>.Failure("Usuario o contraseña incorrectos");

        // Get Roles
        var userRoles = await _rolUsuarioRepository.GetByUserIdAsync(user.IdUsuario);
        var allRoles = await _rolRepository.GetAllAsync();
        var roleNames = new List<string>();
        
        foreach(var ur in userRoles)
        {
            var role = allRoles.FirstOrDefault(r => r.IdRol == ur.IdRol);
            if (role != null) roleNames.Add(role.Nombre);
        }

        var (token, expiration) = GenerateJwtToken(user, roleNames);
        var refreshToken = GenerateRefreshToken();
        
        // Store Refresh Token (In-Memory for now)
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        _cache.Set($"RefreshToken:{refreshToken}", user.IdUsuario, refreshTokenExpiration);

        return Result<LoginResponse>.Success(new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Username = request.Username,
            Expiration = expiration
        });
    }

    public async Task<Result<int>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null) return Result<int>.Failure("El nombre de usuario ya existe");

        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null) return Result<int>.Failure("El correo electrónico ya está registrado");

        var createUserRequest = new CreateUsuarioRequest
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            IdEmpleado = 0 
        };

        try 
        {
            var userId = await _userRepository.AddAsync(createUserRequest);
            return Result<int>.Success(userId);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (!_cache.TryGetValue($"RefreshToken:{request.RefreshToken}", out int userId))
        {
            return Result<LoginResponse>.Failure("Refresh token inválido o expirado");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return Result<LoginResponse>.Failure("Usuario no encontrado");

        var userRoles = await _rolUsuarioRepository.GetByUserIdAsync(user.IdUsuario);
        var allRoles = await _rolRepository.GetAllAsync();
        var roleNames = new List<string>();
        foreach(var ur in userRoles)
        {
            var role = allRoles.FirstOrDefault(r => r.IdRol == ur.IdRol);
            if (role != null) roleNames.Add(role.Nombre);
        }

        var (token, expiration) = GenerateJwtToken(user, roleNames);
        var newRefreshToken = GenerateRefreshToken();

        _cache.Remove($"RefreshToken:{request.RefreshToken}");
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        _cache.Set($"RefreshToken:{newRefreshToken}", user.IdUsuario, refreshTokenExpiration);

        return Result<LoginResponse>.Success(new LoginResponse
        {
            Token = token,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            Expiration = expiration
        });
    }

    public async Task<Result> LogoutAsync(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;
            var duration = expiration - DateTime.UtcNow;

            if (duration > TimeSpan.Zero)
            {
                await _tokenBlacklistService.BlacklistTokenAsync(token, duration);
            }
            return Result.Success();
        }
        catch
        {
            // Ignore invalid token errors on logout
            return Result.Success();
        }
    }

    private (string token, DateTime expiration) GenerateJwtToken(UsuarioResponse user, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("IdUsuario", user.IdUsuario.ToString()),
            new Claim("IdEmpleado", user.IdEmpleado.ToString())
        };
        
        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"]!)),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
