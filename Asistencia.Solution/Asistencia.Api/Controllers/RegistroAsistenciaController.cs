using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRegistroAsistenciaRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRegistroAsistenciaRequest request)
    {
        if (id != request.IdRegistro) return BadRequest("Id mismatch");
        var result = await _service.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return NoContent();
    }
}
