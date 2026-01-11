namespace Asistencia.Shared.DTOs.Requests;

public class CreateEnrolamientoRequest
{
    public int IdEmpleado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
    public string? RutaImagen { get; set; }
    public string? DescriptorFacial { get; set; }
}

public class UpdateEnrolamientoRequest
{
    public int IdEnrolamiento { get; set; }
    public int IdEmpleado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
}
