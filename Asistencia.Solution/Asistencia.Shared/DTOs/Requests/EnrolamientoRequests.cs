namespace Asistencia.Shared.DTOs.Requests;

public class CreateEnrolamientoRequest
{
    public int IdEmpleado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
}

public class UpdateEnrolamientoRequest
{
    public int IdEnrolamiento { get; set; }
    public int IdEmpleado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
}
