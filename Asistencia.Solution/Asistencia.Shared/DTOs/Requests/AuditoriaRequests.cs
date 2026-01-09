namespace Asistencia.Shared.DTOs.Requests;

public class CreateAuditoriaRequest
{
    public string Entidad { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public string? DetalleAnterior { get; set; }
    public string? DetalleNuevo { get; set; }
}

// Usually Auditoria is not updated, but I will provide Update DTO just in case or skip it.
// The SP has UPDATE, so I will provide it.
public class UpdateAuditoriaRequest
{
    public int IdAuditoria { get; set; }
    public string Entidad { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
    public string? DetalleAnterior { get; set; }
    public string? DetalleNuevo { get; set; }
}
