using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de enrolamientos (datos biométricos).
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EnrolamientoController : ControllerBase
{
    private readonly IEnrolamientoService _enrolamientoService;

    public EnrolamientoController(IEnrolamientoService enrolamientoService)
    {
        _enrolamientoService = enrolamientoService;
    }

    /// <summary>
    /// Obtiene todos los enrolamientos.
    /// </summary>
    /// <returns>Lista de enrolamientos.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _enrolamientoService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un enrolamiento por su ID.
    /// </summary>
    /// <param name="id">ID del enrolamiento.</param>
    /// <returns>Datos del enrolamiento.</returns>
    /// <response code="200">Enrolamiento encontrado.</response>
    /// <response code="404">Enrolamiento no encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _enrolamientoService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Crea un nuevo enrolamiento.
    /// </summary>
    /// <param name="request">Datos del nuevo enrolamiento.</param>
    /// <returns>Enrolamiento creado.</returns>
    /// <response code="201">Enrolamiento creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateEnrolamientoRequest request)
    {
        var result = await _enrolamientoService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Actualiza un enrolamiento existente.
    /// </summary>
    /// <param name="id">ID del enrolamiento a actualizar.</param>
    /// <param name="request">Nuevos datos del enrolamiento.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Enrolamiento actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateEnrolamientoRequest request)
    {
        if (id != request.IdEnrolamiento) return BadRequest("Id mismatch");
        var result = await _enrolamientoService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un enrolamiento.
    /// </summary>
    /// <param name="id">ID del enrolamiento a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Enrolamiento eliminado exitosamente.</response>
    /// <response code="400">Error al eliminar.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _enrolamientoService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
