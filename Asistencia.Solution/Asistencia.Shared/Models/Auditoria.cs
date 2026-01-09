namespace Asistencia.Shared.Models;

public class Auditoria
{
    public long IdAuditoria { get; set; }
    public string? Entidad { get; set; }
    public string? Accion { get; set; }
    public int? IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public string? DetalleAnterior { get; set; }
    public string? DetalleNuevo { get; set; }
}
