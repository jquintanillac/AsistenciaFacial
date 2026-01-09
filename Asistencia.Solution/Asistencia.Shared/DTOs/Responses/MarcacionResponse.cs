namespace Asistencia.Shared.DTOs.Responses;

public class MarcacionResponse
{
    public long IdMarcacion { get; set; }
    public int IdEmpleado { get; set; }
    public DateTime FechaHora { get; set; }
    public string TipoMarcacion { get; set; } = string.Empty;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public bool EsValida { get; set; }
}
