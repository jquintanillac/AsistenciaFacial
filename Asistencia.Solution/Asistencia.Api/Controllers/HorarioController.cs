using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de horarios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HorarioController : ControllerBase
{
    private readonly IHorarioService _service;

    public HorarioController(IHorarioService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todos los horarios.
    /// </summary>
    /// <returns>Lista de horarios.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un horario por su ID.
    /// </summary>
    /// <param name="id">ID del horario.</param>
    /// <returns>Datos del horario.</returns>
    /// <response code="200">Horario encontrado.</response>
    /// <response code="404">Horario no encontrado.</response>
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
    /// Crea un nuevo horario.
    /// </summary>
    /// <param name="request">Datos del nuevo horario.</param>
    /// <returns>Horario creado.</returns>
    /// <response code="201">Horario creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateHorarioRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza un horario existente.
    /// </summary>
    /// <param name="id">ID del horario a actualizar.</param>
    /// <param name="request">Nuevos datos del horario.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Horario actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateHorarioRequest request)
    {
        if (id != request.IdHorario) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un horario.
    /// </summary>
    /// <param name="id">ID del horario a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Horario eliminado exitosamente.</response>
    /// <response code="404">Horario no encontrado.</response>
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
