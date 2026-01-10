using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de asignación de horarios a empleados.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HorarioEmpleadoController : ControllerBase
{
    private readonly IHorarioEmpleadoService _horarioEmpleadoService;

    public HorarioEmpleadoController(IHorarioEmpleadoService horarioEmpleadoService)
    {
        _horarioEmpleadoService = horarioEmpleadoService;
    }

    /// <summary>
    /// Obtiene todas las asignaciones de horarios.
    /// </summary>
    /// <returns>Lista de asignaciones de horarios.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _horarioEmpleadoService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una asignación de horario por su ID.
    /// </summary>
    /// <param name="id">ID de la asignación.</param>
    /// <returns>Datos de la asignación de horario.</returns>
    /// <response code="200">Asignación encontrada.</response>
    /// <response code="404">Asignación no encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _horarioEmpleadoService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    /// <summary>
    /// Asigna un horario a un empleado.
    /// </summary>
    /// <param name="request">Datos de la asignación.</param>
    /// <returns>Asignación creada.</returns>
    /// <response code="201">Horario asignado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateHorarioEmpleadoRequest request)
    {
        var result = await _horarioEmpleadoService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Actualiza una asignación de horario existente.
    /// </summary>
    /// <param name="id">ID de la asignación a actualizar.</param>
    /// <param name="request">Nuevos datos de la asignación.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Asignación actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateHorarioEmpleadoRequest request)
    {
        if (id != request.IdHorarioEmpleado) return BadRequest("Id mismatch");
        var result = await _horarioEmpleadoService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina una asignación de horario.
    /// </summary>
    /// <param name="id">ID de la asignación a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Asignación eliminada exitosamente.</response>
    /// <response code="400">Error al eliminar.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _horarioEmpleadoService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
