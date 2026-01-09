namespace Asistencia.Shared.Models;

public class Empleado
{
    public int IdEmpleado { get; set; }
    public int IdEmpresa { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
}
