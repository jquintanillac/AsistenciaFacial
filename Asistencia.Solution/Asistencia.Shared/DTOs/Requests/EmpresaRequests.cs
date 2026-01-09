namespace Asistencia.Shared.DTOs.Requests;

public class CreateEmpresaRequest
{
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? ConfiguracionAsistencia { get; set; }
    public string? Logo { get; set; }
}

public class UpdateEmpresaRequest
{
    public int IdEmpresa { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? ConfiguracionAsistencia { get; set; }
    public string? Logo { get; set; }
}
