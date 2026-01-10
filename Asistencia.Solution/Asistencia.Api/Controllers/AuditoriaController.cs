using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de auditoría y logs.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    /// <summary>
    /// Obtiene todos los registros de auditoría.
    /// </summary>
    /// <returns>Lista de registros de auditoría.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _auditoriaService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un registro de auditoría por su ID.
    /// </summary>
    /// <param name="id">ID del registro.</param>
    /// <returns>Datos del registro de auditoría.</returns>
    /// <response code="200">Registro encontrado.</response>
    /// <response code="404">Registro no encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _auditoriaService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Crea un nuevo registro de auditoría.
    /// </summary>
    /// <param name="request">Datos del nuevo registro.</param>
    /// <returns>Registro creado.</returns>
    /// <response code="201">Registro creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateAuditoriaRequest request)
    {
        var result = await _auditoriaService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Actualiza un registro de auditoría existente.
    /// </summary>
    /// <param name="id">ID del registro a actualizar.</param>
    /// <param name="request">Nuevos datos del registro.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Registro actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, UpdateAuditoriaRequest request)
    {
        if (id != request.IdAuditoria) return BadRequest("Id mismatch");
        var result = await _auditoriaService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un registro de auditoría.
    /// </summary>
    /// <param name="id">ID del registro a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Registro eliminado exitosamente.</response>
    /// <response code="400">Error al eliminar.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _auditoriaService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
