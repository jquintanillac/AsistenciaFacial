namespace Asistencia.Shared.DTOs.Responses;

public class AuditoriaResponse
{
    public long IdAuditoria { get; set; }
    public string Entidad { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public string? DetalleAnterior { get; set; }
    public string? DetalleNuevo { get; set; }
}
