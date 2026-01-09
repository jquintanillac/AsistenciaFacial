using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _horarioEmpleadoService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _horarioEmpleadoService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHorarioEmpleadoRequest request)
    {
        var result = await _horarioEmpleadoService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateHorarioEmpleadoRequest request)
    {
        if (id != request.IdHorarioEmpleado) return BadRequest("Id mismatch");
        var result = await _horarioEmpleadoService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _horarioEmpleadoService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
