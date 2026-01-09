namespace Asistencia.Shared.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public int? IdEmpleado { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Estado { get; set; }
}
