using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de ubicaciones permitidas para marcación.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UbicacionPermitidaController : ControllerBase
{
    private readonly IUbicacionPermitidaService _service;

    public UbicacionPermitidaController(IUbicacionPermitidaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todas las ubicaciones permitidas.
    /// </summary>
    /// <returns>Lista de ubicaciones permitidas.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una ubicación permitida por su ID.
    /// </summary>
    /// <param name="id">ID de la ubicación.</param>
    /// <returns>Datos de la ubicación permitida.</returns>
    /// <response code="200">Ubicación encontrada.</response>
    /// <response code="404">Ubicación no encontrada.</response>
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
    /// Crea una nueva ubicación permitida.
    /// </summary>
    /// <param name="request">Datos de la nueva ubicación.</param>
    /// <returns>Ubicación creada.</returns>
    /// <response code="201">Ubicación creada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateUbicacionPermitidaRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza una ubicación permitida existente.
    /// </summary>
    /// <param name="id">ID de la ubicación a actualizar.</param>
    /// <param name="request">Nuevos datos de la ubicación.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Ubicación actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateUbicacionPermitidaRequest request)
    {
        if (id != request.IdUbicacion) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina una ubicación permitida.
    /// </summary>
    /// <param name="id">ID de la ubicación a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Ubicación eliminada exitosamente.</response>
    /// <response code="404">Ubicación no encontrada.</response>
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
