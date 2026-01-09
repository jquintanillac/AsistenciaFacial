namespace Asistencia.Shared.DTOs.Responses;

public class UsuarioResponse
{
    public int IdUsuario { get; set; }
    public int IdEmpleado { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Estado { get; set; }
}
