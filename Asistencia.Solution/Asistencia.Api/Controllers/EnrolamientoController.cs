using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _enrolamientoService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _enrolamientoService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrolamientoRequest request)
    {
        var result = await _enrolamientoService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEnrolamientoRequest request)
    {
        if (id != request.IdEnrolamiento) return BadRequest("Id mismatch");
        var result = await _enrolamientoService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _enrolamientoService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
