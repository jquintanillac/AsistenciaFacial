using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de marcaciones de asistencia.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MarcacionController : ControllerBase
{
    private readonly IMarcacionService _marcacionService;

    public MarcacionController(IMarcacionService marcacionService)
    {
        _marcacionService = marcacionService;
    }

    /// <summary>
    /// Obtiene todas las marcaciones.
    /// </summary>
    /// <returns>Lista de marcaciones.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _marcacionService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una marcación por su ID.
    /// </summary>
    /// <param name="id">ID de la marcación.</param>
    /// <returns>Datos de la marcación.</returns>
    /// <response code="200">Marcación encontrada.</response>
    /// <response code="404">Marcación no encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _marcacionService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene las marcaciones de un empleado específico.
    /// </summary>
    /// <param name="employeeId">ID del empleado.</param>
    /// <returns>Lista de marcaciones del empleado.</returns>
    /// <response code="200">Marcaciones encontradas.</response>
    /// <response code="400">Error en la solicitud.</response>
    [HttpGet("empleado/{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _marcacionService.GetByEmployeeIdAsync(employeeId);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Crea una nueva marcación.
    /// </summary>
    /// <param name="request">Datos de la nueva marcación.</param>
    /// <returns>Marcación creada.</returns>
    /// <response code="201">Marcación creada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateMarcacionRequest request)
    {
        var result = await _marcacionService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Actualiza una marcación existente.
    /// </summary>
    /// <param name="id">ID de la marcación a actualizar.</param>
    /// <param name="request">Nuevos datos de la marcación.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Marcación actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, UpdateMarcacionRequest request)
    {
        if (id != request.IdMarcacion) return BadRequest("Id mismatch");
        var result = await _marcacionService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina una marcación.
    /// </summary>
    /// <param name="id">ID de la marcación a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Marcación eliminada exitosamente.</response>
    /// <response code="400">Error al eliminar.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _marcacionService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
