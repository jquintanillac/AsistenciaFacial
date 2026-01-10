using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de roles de usuario.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolController(IRolService rolService)
    {
        _rolService = rolService;
    }

    /// <summary>
    /// Obtiene todos los roles.
    /// </summary>
    /// <returns>Lista de roles.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _rolService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un rol por su ID.
    /// </summary>
    /// <param name="id">ID del rol.</param>
    /// <returns>Datos del rol.</returns>
    /// <response code="200">Rol encontrado.</response>
    /// <response code="404">Rol no encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _rolService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Crea un nuevo rol.
    /// </summary>
    /// <param name="request">Datos del nuevo rol.</param>
    /// <returns>Rol creado.</returns>
    /// <response code="201">Rol creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateRolRequest request)
    {
        var result = await _rolService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Actualiza un rol existente.
    /// </summary>
    /// <param name="id">ID del rol a actualizar.</param>
    /// <param name="request">Nuevos datos del rol.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Rol actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateRolRequest request)
    {
        if (id != request.IdRol) return BadRequest("Id mismatch");
        var result = await _rolService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un rol.
    /// </summary>
    /// <param name="id">ID del rol a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Rol eliminado exitosamente.</response>
    /// <response code="400">Error al eliminar.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _rolService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
