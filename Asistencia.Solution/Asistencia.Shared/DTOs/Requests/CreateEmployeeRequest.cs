namespace Asistencia.Shared.DTOs.Requests;

public class CreateEmployeeRequest
{
    public int IdEmpresa { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Cargo { get; set; }
}
