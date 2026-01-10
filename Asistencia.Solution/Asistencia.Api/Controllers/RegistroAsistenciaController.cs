using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de registros de asistencia.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RegistroAsistenciaController : ControllerBase
{
    private readonly IRegistroAsistenciaService _service;

    public RegistroAsistenciaController(IRegistroAsistenciaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todos los registros de asistencia.
    /// </summary>
    /// <returns>Lista de registros de asistencia.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un registro de asistencia por su ID.
    /// </summary>
    /// <param name="id">ID del registro.</param>
    /// <returns>Datos del registro de asistencia.</returns>
    /// <response code="200">Registro encontrado.</response>
    /// <response code="404">Registro no encontrado.</response>
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
    /// Crea un nuevo registro de asistencia.
    /// </summary>
    /// <param name="request">Datos del nuevo registro.</param>
    /// <returns>Registro creado.</returns>
    /// <response code="201">Registro creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateRegistroAsistenciaRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza un registro de asistencia existente.
    /// </summary>
    /// <param name="id">ID del registro a actualizar.</param>
    /// <param name="request">Nuevos datos del registro.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Registro actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateRegistroAsistenciaRequest request)
    {
        if (id != request.IdRegistro) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un registro de asistencia.
    /// </summary>
    /// <param name="id">ID del registro a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Registro eliminado exitosamente.</response>
    /// <response code="404">Registro no encontrado.</response>
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
