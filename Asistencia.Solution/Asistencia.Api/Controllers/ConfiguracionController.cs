using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConfiguracionController : ControllerBase
{
    private readonly IConfiguracionService _service;

    public ConfiguracionController(IConfiguracionService service)
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
    public async Task<IActionResult> Create(CreateConfiguracionRequest request)
    {
        var result = await _service.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateConfiguracionRequest request)
    {
        if (id != request.IdConfiguracion) return BadRequest("Id mismatch");
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
