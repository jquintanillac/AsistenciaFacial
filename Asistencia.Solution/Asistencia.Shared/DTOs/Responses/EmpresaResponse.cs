namespace Asistencia.Shared.DTOs.Responses;

public class EmpresaResponse
{
    public int IdEmpresa { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? ConfiguracionAsistencia { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}
