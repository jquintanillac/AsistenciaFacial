using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _auditoriaService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _auditoriaService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAuditoriaRequest request)
    {
        var result = await _auditoriaService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateAuditoriaRequest request)
    {
        if (id != request.IdAuditoria) return BadRequest("Id mismatch");
        var result = await _auditoriaService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _auditoriaService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
