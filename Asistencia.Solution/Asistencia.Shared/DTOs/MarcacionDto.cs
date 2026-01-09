namespace Asistencia.Shared.DTOs;

public class MarcacionDto
{
    public long IdMarcacion { get; set; }
    public int IdEmpleado { get; set; }
    public DateTime FechaHora { get; set; }
    public string? TipoMarcacion { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool EsValida { get; set; }
}
