namespace Asistencia.Shared.DTOs;

public class UbicacionPermitidaDto
{
    public int IdUbicacion { get; set; }
    public int IdEmpresa { get; set; }
    public string? Nombre { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public int RadioMetros { get; set; }
}
