using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de empleados.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todos los empleados.
    /// </summary>
    /// <returns>Lista de empleados.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene un empleado por su ID.
    /// </summary>
    /// <param name="id">ID del empleado.</param>
    /// <returns>Datos del empleado.</returns>
    /// <response code="200">Empleado encontrado.</response>
    /// <response code="404">Empleado no encontrado.</response>
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
    /// Crea un nuevo empleado.
    /// </summary>
    /// <param name="request">Datos del nuevo empleado.</param>
    /// <returns>Empleado creado.</returns>
    /// <response code="201">Empleado creado exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateEmployeeRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza un empleado existente.
    /// </summary>
    /// <param name="id">ID del empleado a actualizar.</param>
    /// <param name="request">Nuevos datos del empleado.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Empleado actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateEmployeeRequest request)
    {
        if (id != request.IdEmpleado) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina un empleado.
    /// </summary>
    /// <param name="id">ID del empleado a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Empleado eliminado exitosamente.</response>
    /// <response code="404">Empleado no encontrado.</response>
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
