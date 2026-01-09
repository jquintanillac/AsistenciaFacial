using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la autenticación de usuarios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;

    public AuthController(IAuthService authService, IValidator<RegisterRequest> registerValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
    }

    /// <summary>
    /// Inicia sesión de un usuario y devuelve un token JWT.
    /// </summary>
    /// <param name="request">Credenciales de inicio de sesión.</param>
    /// <returns>Token JWT, Refresh Token y detalles del usuario.</returns>
    /// <response code="200">Inicio de sesión exitoso.</response>
    /// <response code="401">Credenciales inválidas.</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.IsSuccess) return Unauthorized(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request">Datos del usuario a registrar.</param>
    /// <returns>ID del usuario creado.</returns>
    /// <response code="200">Usuario registrado exitosamente.</response>
    /// <response code="400">Datos inválidos o usuario ya existente.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var result = await _authService.RegisterAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { IdUsuario = result.Value });
    }

    /// <summary>
    /// Renueva el token de acceso utilizando un refresh token válido.
    /// </summary>
    /// <param name="request">Token de acceso actual y refresh token.</param>
    /// <returns>Nuevo token de acceso y nuevo refresh token.</returns>
    /// <response code="200">Token renovado exitosamente.</response>
    /// <response code="401">Refresh token inválido o expirado.</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (!result.IsSuccess) return Unauthorized(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Cierra la sesión del usuario actual y revoca el token de acceso (Blacklist).
    /// </summary>
    /// <returns>No content.</returns>
    /// <response code="204">Cierre de sesión exitoso.</response>
    /// <response code="400">Token faltante o error al procesar.</response>
    /// <response code="401">No autorizado.</response>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token)) return BadRequest("Token is missing");

        var result = await _authService.LogoutAsync(token);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
