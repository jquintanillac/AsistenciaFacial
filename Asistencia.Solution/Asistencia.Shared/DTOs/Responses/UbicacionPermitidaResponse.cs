namespace Asistencia.Shared.DTOs.Responses;

public class UbicacionPermitidaResponse
{
    public int IdUbicacion { get; set; }
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public int RadioMetros { get; set; }
}
