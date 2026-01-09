using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MarcacionController : ControllerBase
{
    private readonly IMarcacionService _marcacionService;

    public MarcacionController(IMarcacionService marcacionService)
    {
        _marcacionService = marcacionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _marcacionService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _marcacionService.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("empleado/{employeeId}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _marcacionService.GetByEmployeeIdAsync(employeeId);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMarcacionRequest request)
    {
        var result = await _marcacionService.CreateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateMarcacionRequest request)
    {
        if (id != request.IdMarcacion) return BadRequest("Id mismatch");
        var result = await _marcacionService.UpdateAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _marcacionService.DeleteAsync(id);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
