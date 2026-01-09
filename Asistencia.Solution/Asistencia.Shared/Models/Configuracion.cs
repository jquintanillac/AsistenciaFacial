namespace Asistencia.Shared.Models;

public class Configuracion
{
    public int IdConfiguracion { get; set; }
    public int IdEmpresa { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Logo { get; set; }
}
