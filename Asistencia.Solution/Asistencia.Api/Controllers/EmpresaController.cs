using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

/// <summary>
/// Controlador para la gestión de empresas.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _service;

    public EmpresaController(IEmpresaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtiene todas las empresas registradas.
    /// </summary>
    /// <returns>Lista de empresas.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una empresa por su ID.
    /// </summary>
    /// <param name="id">ID de la empresa.</param>
    /// <returns>Datos de la empresa.</returns>
    /// <response code="200">Empresa encontrada.</response>
    /// <response code="404">Empresa no encontrada.</response>
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
    /// Crea una nueva empresa.
    /// </summary>
    /// <param name="request">Datos de la nueva empresa.</param>
    /// <returns>Empresa creada.</returns>
    /// <response code="201">Empresa creada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateEmpresaRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    /// <summary>
    /// Actualiza una empresa existente.
    /// </summary>
    /// <param name="id">ID de la empresa a actualizar.</param>
    /// <param name="request">Nuevos datos de la empresa.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Empresa actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos o ID no coincide.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateEmpresaRequest request)
    {
        if (id != request.IdEmpresa) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Elimina una empresa.
    /// </summary>
    /// <param name="id">ID de la empresa a eliminar.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Empresa eliminada exitosamente.</response>
    /// <response code="404">Empresa no encontrada.</response>
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
