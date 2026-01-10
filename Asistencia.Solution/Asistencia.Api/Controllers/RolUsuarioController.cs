using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de asignación de roles a usuarios.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolUsuarioController : ControllerBase
{
    private readonly IRolUsuarioService _rolUsuarioService;

    public RolUsuarioController(IRolUsuarioService rolUsuarioService)
    {
        _rolUsuarioService = rolUsuarioService;
    }

    /// <summary>
    /// Obtiene todas las asignaciones de roles a usuarios.
    /// </summary>
    /// <returns>Lista de asignaciones.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _rolUsuarioService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene los roles asignados a un usuario específico.
    /// </summary>
    /// <param name="idUsuario">ID del usuario.</param>
    /// <returns>Roles del usuario.</returns>
    /// <response code="200">Roles encontrados.</response>
    /// <response code="404">Usuario no encontrado o sin roles.</response>
    [HttpGet("usuario/{idUsuario}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(int idUsuario)
    {
        var result = await _rolUsuarioService.GetByUserIdAsync(idUsuario);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Asigna un rol a un usuario.
    /// </summary>
    /// <param name="request">Datos de la asignación.</param>
    /// <returns>OK si se asignó correctamente.</returns>
    /// <response code="200">Rol asignado exitosamente.</response>
    /// <response code="400">Datos inválidos o error al asignar.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRole(CreateRolUsuarioRequest request)
    {
        var result = await _rolUsuarioService.AssignRoleAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok();
    }

    /// <summary>
    /// Remueve un rol de un usuario.
    /// </summary>
    /// <param name="idUsuario">ID del usuario.</param>
    /// <param name="idRol">ID del rol.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Rol removido exitosamente.</response>
    /// <response code="400">Error al remover.</response>
    [HttpDelete("{idUsuario}/{idRol}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveRole(int idUsuario, int idRol)
    {
        var result = await _rolUsuarioService.RemoveRoleAsync(idUsuario, idRol);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
