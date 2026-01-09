namespace Asistencia.Shared.DTOs.Requests;

public class CreateUsuarioRequest
{
    public int IdEmpleado { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateUsuarioRequest
{
    public int IdUsuario { get; set; }
    public int IdEmpleado { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; } // Optional if not changing password
}
