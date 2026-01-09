using Asistencia.Application.Services;
using Asistencia.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asistencia.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolUsuarioController : ControllerBase
{
    private readonly IRolUsuarioService _rolUsuarioService;

    public RolUsuarioController(IRolUsuarioService rolUsuarioService)
    {
        _rolUsuarioService = rolUsuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _rolUsuarioService.GetAllAsync();
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> GetByUserId(int idUsuario)
    {
        var result = await _rolUsuarioService.GetByUserIdAsync(idUsuario);
        if (!result.IsSuccess) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(CreateRolUsuarioRequest request)
    {
        var result = await _rolUsuarioService.AssignRoleAsync(request);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok();
    }

    [HttpDelete("{idUsuario}/{idRol}")]
    public async Task<IActionResult> RemoveRole(int idUsuario, int idRol)
    {
        var result = await _rolUsuarioService.RemoveRoleAsync(idUsuario, idRol);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return NoContent();
    }
}
