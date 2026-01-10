using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de configuraciones del sistema.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConfiguracionController : ControllerBase
{
    private readonly IConfiguracionService _service;

    public ConfiguracionController(IConfiguracionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todas las configuraciones.
    /// </summary>
    /// <returns>Lista de configuraciones.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una configuración por su ID.
    /// </summary>
    /// <param name="id">ID de la configuración.</param>
    /// <returns>Datos de la configuración.</returns>
    /// <response code="200">Configuración encontrada.</response>
    /// <response code="404">Configuración no encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Crea una nueva configuración.
    /// </summary>
    /// <param name="request">Datos de la nueva configuración.</param>
    /// <returns>Configuración creada.</returns>
    /// <response code="201">Configuración creada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateConfiguracionRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza una configuración existente.
    /// </summary>
    /// <param name="id">ID de la configuración a actualizar.</param>
    /// <param name="request">Nuevos datos de la configuración.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Configuración actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateConfiguracionRequest request)
    {
        if (id != request.IdConfiguracion) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina una configuración.
    /// </summary>
    /// <param name="id">ID de la configuración a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Configuración eliminada exitosamente.</response>
    /// <response code="404">Configuración no encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return NoContent();
    }
}
