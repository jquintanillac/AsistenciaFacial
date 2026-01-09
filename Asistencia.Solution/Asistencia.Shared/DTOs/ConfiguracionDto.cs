namespace Asistencia.Shared.DTOs;

public class ConfiguracionDto
{
    public int IdConfiguracion { get; set; }
    public int IdEmpresa { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Logo { get; set; }
}
